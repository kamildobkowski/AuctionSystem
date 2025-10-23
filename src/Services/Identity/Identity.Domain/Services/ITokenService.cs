using Identity.Domain.Entities;
using Shared.Base.Result;

namespace Identity.Domain.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken(User user);
        Task<Result<(string AccessToken, string RefreshToken)>> RefreshAsync(string refreshToken);
    }
}