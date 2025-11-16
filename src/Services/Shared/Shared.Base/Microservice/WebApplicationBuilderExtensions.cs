using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Base.Errors;
using Shared.Base.Token;

namespace Shared.Base.Microservice;

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder UseMicroservice(this WebApplicationBuilder builder)
	{
		builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = false,

					SignatureValidator = (token, parameters) => new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token),
					RequireSignedTokens = false
				};
				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = ctx =>
					{
						Console.WriteLine($"JWT fail: {ctx.Exception.Message}");
						return Task.CompletedTask;
					},
					OnChallenge = ctx =>
					{
						Console.WriteLine($"JWT challenge: {ctx.Error} - {ctx.ErrorDescription}");
						return Task.CompletedTask;
					}
				};
				options.IncludeErrorDetails = true;
			});
		
		builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
		builder.Services.AddControllers(options =>
			{
				options.Filters.Add(new AuthorizeFilter( new AuthorizationPolicyBuilder()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build()));
				options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), 500));
				options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResult), 400));
				options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
			})
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				options.JsonSerializerOptions.WriteIndented = true;
				options.JsonSerializerOptions.AllowTrailingCommas = true;
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
			});
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(option =>
		{
			option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter a valid token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});
			option.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new List<string>()
				}
			});
			option.UseInlineDefinitionsForEnums();
		});
		
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddScoped<IUserContextProvider, UserContextProvider>();
		
		return builder;
	}
}