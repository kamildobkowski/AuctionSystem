using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.Services;
using Identity.Infrastructure.Configuration;
using Shared.Base.Errors;
using Shared.Base.Result;

namespace Identity.Infrastructure.Token;
public class TokenService(
    IUserRepository userRepository,
    JwtConfiguration config,
    IRefreshTokenHasher refreshTokenHasher)
    : ITokenService
{
    public string GenerateAccessToken(User user)
    {
        var now = DateTime.UtcNow;
        var expires = now.Add(config.AccessTokenLifetime);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.GetName()),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var keyBytes = Encoding.UTF8.GetBytes(config.Secret);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            IssuedAt = now,
            NotBefore = now,
            SigningCredentials = signingCredentials,
            Issuer = string.IsNullOrWhiteSpace(config.Issuer) ? null : config.Issuer,
            Audience = string.IsNullOrWhiteSpace(config.Audience) ? null : config.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(securityToken);

        return tokenString;
    }

    public async Task<string> GenerateRefreshToken(User user)
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(bytes);
        var hashedToken = refreshTokenHasher.Hash(refreshToken);
        user.SetRefreshToken(hashedToken, DateTime.UtcNow.AddDays(30));
        await userRepository.SaveChangesAsync();
        return refreshToken;
    }        

    public async Task<Result<(string AccessToken, string RefreshToken)>> RefreshAsync(string refreshToken)
    {
        var lookupHash = refreshTokenHasher.Hash(refreshToken);
        var user = await userRepository.GetByRefreshToken(lookupHash);
        if (user is null 
            || !user.RefreshTokenExpiresAt.HasValue 
            || user.RefreshTokenExpiresAt.Value < DateTime.UtcNow)
            return Result.Failure<(string, string)>(ErrorResult.UnauthorizedError);

        var newAccess = GenerateAccessToken(user);
        var newRefreshPlain = await GenerateRefreshToken(user);

        return Result.Ok((newAccess, newRefreshPlain));
    }
 }