using Identity.Application.Repositories;
using Identity.Application.Services;
using Identity.Domain.ValueObjects;
using StackExchange.Redis;

namespace Identity.Infrastructure.Cache;

public class CacheService(IUserRepository userRepository, IDatabase cache) : ICacheService
{
	private const string TakenEmailKey = "takenEmail";
	private const string VerificationCodeKey = "verificationCode";
	private const string TakenPhoneNumberKey = "takenPhoneNumber";
	
	public async Task<bool> IsEmailTaken(string email)
		=> await cache.HashExistsAsync(TakenEmailKey, email) || 
		   await userRepository.Exists(x => x.Email == email);

	public async Task SetEmailTaken(string email)
		=> await cache.HashSetAsync(TakenEmailKey, email, 1);
	
	public Task<bool> IsActivationCodeTaken(string code)
		=> cache.HashExistsAsync(VerificationCodeKey, code);

	public Task SetActivationCode(string userId, string code)
		=> cache.HashSetAsync(VerificationCodeKey, code, userId);
}