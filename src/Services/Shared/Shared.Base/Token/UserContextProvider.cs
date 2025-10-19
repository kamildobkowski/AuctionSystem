using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Shared.Base.Token;

public sealed class UserContextProvider(IHttpContextAccessor httpContextAccessor) : IUserContextProvider
{
	public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

	public Guid GetUserId()
	{
		if (User is null)
			throw new AuthenticationFailureException("User is not authenticated");

		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
		if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
			throw new AuthenticationFailureException("User is not authenticated");
		return userId;
	}

	public string? GetUserName()
	{
		return User?.Identity?.Name;
	}

	public IEnumerable<Claim> GetClaims()
	{
		return User?.Claims ?? [];
	}
}