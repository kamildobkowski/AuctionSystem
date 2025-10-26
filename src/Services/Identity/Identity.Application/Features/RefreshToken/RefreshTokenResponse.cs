namespace Identity.Application.Features.RefreshToken;

public record RefreshTokenResponse(string AccessToken, DateTime AccessTokenExpireDate, string RefreshToken, DateTime RefreshTokenExpireDate);