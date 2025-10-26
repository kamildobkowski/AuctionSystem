using Identity.Domain.Entities;
using Shared.Base.Result;

namespace Identity.Domain.Services
{
    public interface ITokenService
    {
        (string Token, DateTime Expires) GenerateAccessToken(User user);
        Task<(string Token, DateTime Expires)> GenerateRefreshToken(User user);
        Task<Result<(string AccessToken, DateTime AccessExpires, string RefreshToken, DateTime RefreshExpires)>> RefreshAsync(string refreshToken);
    }
}