using System.Security.Claims;

namespace Shared.Base.Token;

public interface IUserContextProvider
{
	ClaimsPrincipal? User { get; }
	Guid GetUserId();
	string? GetUserName();
	IEnumerable<Claim> GetClaims();
}