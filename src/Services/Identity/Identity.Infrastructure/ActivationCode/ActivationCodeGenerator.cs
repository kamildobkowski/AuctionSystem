using System.Security.Cryptography;
using Identity.Domain.Entities;
using Identity.Domain.Services;
using Identity.Infrastructure.Email;

namespace Identity.Infrastructure.ActivationCode;

public class ActivationCodeGenerator(ICacheService cacheService, IEmailService emailService) 
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
		await emailService.SendAccountConfirmationEmail(user.Email, code);
	}
	
	private static string GenerateCode()
	{
		const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
		                       "abcdefghijklmnopqrstuvwxyz" +
		                       "0123456789";
		return RandomNumberGenerator.GetString(allowed, 30);
	}
}