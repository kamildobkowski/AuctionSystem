using FluentValidation;
using Identity.Application.Features.Login;
using Identity.Application.Features.RefreshToken;
using Identity.Application.Features.RegisterUser.Common.Models;
using Identity.Application.Features.RegisterUser.Common.Validators;
using Identity.Application.Features.RegisterUser.RegisterCompanyUser;
using Identity.Application.Features.RegisterUser.RegisterPersonalUser;
using Identity.Application.Features.UserData.GetUserData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Queries;

namespace Identity.Application;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		#region Handlers
		services.AddScoped<ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse>, RegisterPersonalUserCommandHandler>();
		services.AddScoped<ICommandHandler<RegisterCompanyUserCommand, RegisterCompanyUserResponse>, RegisterCompanyUserCommandHandler>();
		services.AddScoped<ICommandHandler<LoginCommand, LoginResponse>, LoginCommandHandler>();
		services.AddScoped<ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>, RefreshTokenCommandHandler>();
		services.AddScoped<IQueryHandler<GetUserDataQuery, GetUserDataQueryResponse>, GetUserDataQueryHandler>();
		#endregion

		#region Validators
		services.AddScoped<IValidator<RegisterPersonalUserCommand>, RegisterPersonalUserCommandValidator>();
		services.AddScoped<IValidator<RegisterCompanyUserCommand>, RegisterCompanyUserCommandValidator>();
		services.AddScoped<IValidator<AddressModel>, AddressModelValidator>();
		services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
		services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();
		#endregion
		
		return services;
	}
}