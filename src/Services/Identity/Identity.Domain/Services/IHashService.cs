using Identity.Domain.Entities;

namespace Identity.Domain.Services;

public interface IHashService
{
	string HashPassword(string password);
	bool VerifyHash(User user, string password);
}