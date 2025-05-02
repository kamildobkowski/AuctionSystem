using System.Security.Cryptography;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Services;
using Shared.Events.EventBus;
using Shared.Events.Events;

namespace Identity.Infrastructure.ActivationCode;

public class ActivationCodeGenerator(ICacheService cacheService, IEventBus eventBus) 
	: IActivationCodeGenerator
{
	public async Task GenerateAndStoreCodeAsync(User user)
	{
		string code;
		do
		{
			code = GenerateCode();
		} while (await cacheService.IsActivationCodeTaken(code));

		await cacheService.SetActivationCode(user.Id.ToString(), code);
		await eventBus.PublishAsync(new EmailVerificationRequiredEvent { Code = code, Email = user.Email });
	}
	
	private static string GenerateCode()
	{
		const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
		                       "abcdefghijklmnopqrstuvwxyz" +
		                       "0123456789";
		return RandomNumberGenerator.GetString(allowed, 30);
	}
}