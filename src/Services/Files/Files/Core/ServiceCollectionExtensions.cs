using Files.Core.Configuration;
using Files.Core.Persistence;
using Files.Core.RecurringJobs;
using Files.Core.Storage;
using Files.Features.Images.SetImageToUsed;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Shared.Events.Events.Files;

namespace Files.Core;

public static class ServiceCollectionExtensions
{
	public static void AddCore(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ImagesDbContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("postgres")));
			
		var jwtConfig = new ImageConfiguration();
		configuration.GetSection("Image").Bind(jwtConfig);
		services.AddSingleton(jwtConfig);
		
		var fileStorageConfig = new FileStorageConfiguration();
		configuration.GetSection("FileStorage").Bind(fileStorageConfig);
		services.AddSingleton(fileStorageConfig);

		services.AddScoped<IFileStorageService, MinioStorageService>();
		services.AddScoped<IRecurringJobsPopulator, RecurringJobsPopulator>();
		
		services.AddHangfire(config => config
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UsePostgreSqlStorage(options =>
			{
				options.UseNpgsqlConnection(configuration.GetConnectionString("postgres"));
			}));
			
		services.AddMinioSetup(configuration);
		services.SetupMassTransit(configuration);
	}

	private static void AddMinioSetup(this IServiceCollection services, IConfiguration configuration)
	{
		var minioSection = configuration.GetSection("Minio");
		var endpoint = minioSection["Endpoint"] ?? throw new InvalidOperationException("MinIO endpoint missing.");
		var accessKey = minioSection["AccessKey"] ?? throw new InvalidOperationException("MinIO access key missing.");
		var secretKey = minioSection["SecretKey"] ?? throw new InvalidOperationException("MinIO secret key missing.");
		services.AddMinio(client =>
		{
			client.WithEndpoint(endpoint)
				.WithCredentials(accessKey, secretKey)
				.WithSSL(false);
		});
	}

	public static async Task PrepareMinio(this WebApplication app)
	{
		var client = app.Services.GetRequiredService<IMinioClient>();
		const string bucketName = Buckets.ImagesBucket;

        var bucketExists = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
        if (!bucketExists)
        {
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }

        var publicReadPolicy = $$"""

                                 {
                                   "Version": "2012-10-17",
                                   "Statement": [
                                     {
                                       "Effect": "Allow",
                                       "Principal": { "AWS": [ "*" ] },
                                       "Action": [ "s3:GetObject" ],
                                       "Resource": [ "arn:aws:s3:::{{bucketName}}/*" ]
                                     }
                                   ]
                                 }
                                 """;

        await client.SetPolicyAsync(new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(publicReadPolicy));
    }
	
	public static void PopulateRecurringJobs(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var populator = scope.ServiceProvider.GetRequiredService<IRecurringJobsPopulator>();
		populator.Populate();
	}
	
	private static IServiceCollection SetupMassTransit(this IServiceCollection services, IConfiguration configuration)
	{
		var consumerGroup = configuration["AppName"];
		var connectionString = configuration.GetConnectionString("kafka");
		Console.WriteLine(connectionString);
		services.AddMassTransit(x =>
		{
			x.UsingInMemory();
			x.AddRider(rider =>
			{
				rider.UsingKafka((context, cfg) =>
				{
					cfg.Host(connectionString);
					cfg.TopicEndpoint<SetImageToUsedCommand>(SetImageToUsedCommand.Topic, consumerGroup, x =>
					{
						x.CreateIfMissing();
						x.ConfigureConsumer<SetImageToUsedCommandConsumer>(context);
					});
				});
				rider.AddConsumer<SetImageToUsedCommandConsumer>();
			});
		});
		
		return services;
	}
}