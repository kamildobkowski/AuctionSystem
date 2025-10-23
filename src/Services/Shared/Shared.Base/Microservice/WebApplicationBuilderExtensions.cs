using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base.Token;

namespace Shared.Base.Microservice;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder UseMicroservice(this WebApplicationBuilder builder)
	{
		builder.Services.AddAuthorizationBuilder()
            .SetFallbackPolicy(
	            new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.Build());
		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddScoped<IUserContextProvider, UserContextProvider>();
		
		return builder;
	}
}