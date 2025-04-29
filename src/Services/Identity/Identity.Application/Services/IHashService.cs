using Identity.Domain.Entities;

namespace Identity.Application.Services;

public interface IHashService
{
	string Hash(string password);
	bool Verify(User user, string hash);
}