using Identity.Application.Repositories;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Services;
using Identity.Infrastructure.ActivationCode;
using Identity.Infrastructure.Cache;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Passwords;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Identity.Infrastructure;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<UserDbContext>(x => 
			x.UseNpgsql(configuration.GetConnectionString("postgres")));
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
		return services;
	}
}