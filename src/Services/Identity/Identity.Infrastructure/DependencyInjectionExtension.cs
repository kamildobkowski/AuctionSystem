using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.Services;
using Identity.Infrastructure.ActivationCode;
using Identity.Infrastructure.Cache;
using Identity.Infrastructure.Configuration;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Email;
using Identity.Infrastructure.Passwords;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.Infrastructure.Token;
using MassTransit;
using Shared.Events.Events.Users;
using StackExchange.Redis;

namespace Identity.Infrastructure;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<UserDbContext>(x => 
			x.UseNpgsql(configuration.GetConnectionString("postgres")));
		
		services.AddMassTransit(x =>
		{
			x.UsingInMemory();
			x.AddEntityFrameworkOutbox<UserDbContext>(x =>
			{
				x.QueryDelay = TimeSpan.FromSeconds(1);
				x.DuplicateDetectionWindow = TimeSpan.FromMinutes(10);
				x.UsePostgres();
			});
			x.AddRider(rider =>
			{
				rider.AddProducer<PersonalUserCreatedEvent>(PersonalUserCreatedEvent.Topic);
				rider.UsingKafka((context, configurator) =>
				{
					configurator.Host("vpn.kamildobkowski.pl:9092");
				});
			});
		});
		
		services.AddSingleton<IDatabase>(x =>
		{
			var cfg = configuration.GetConnectionString("Redis");
			return ConnectionMultiplexer.Connect(cfg!).GetDatabase();
		});
		
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IHashService, HashService>();
		services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
		services.AddScoped<ICacheService, CacheService>();
		services.AddScoped<IActivationCodeGenerator, ActivationCodeGenerator>();

		// Jwt configuration and token service
		var jwtSection = configuration.GetSection("JwtConfiguration");
		var jwtConfig = new JwtConfiguration();
		jwtSection.Bind(jwtConfig);
		services.AddSingleton(jwtConfig);
		services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IEmailService, EmailService>();
		return services;
	}
}