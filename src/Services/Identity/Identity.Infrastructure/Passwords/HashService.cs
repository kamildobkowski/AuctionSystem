using System.Text;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Passwords;

public class HashService(IPasswordHasher<User> passwordHasher) : IHashService
{
	public string HashPassword(string password)
	{
		var result = passwordHasher.HashPassword(null!, password);
		return result;
	}
	
	public bool VerifyHash(User user, string hash)
	{
		var result = passwordHasher.VerifyHashedPassword(null!, hash, user.PasswordHash);
		return result == PasswordVerificationResult.Success;
	}
}