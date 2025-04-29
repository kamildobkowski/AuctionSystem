using FluentValidation;
using Identity.Application.Features.RegisterPersonalUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Cqrs.Commands;

namespace Identity.Application;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		#region Handlers
		services.AddScoped<ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse>, RegisterPersonalUserCommandHandler>();
		#endregion

		#region Validators
		services.AddScoped<IValidator<RegisterPersonalUserCommand>, RegisterPersonalUserCommandValidator>();
		#endregion
		
		return services;
	}
}