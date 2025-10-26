namespace Identity.Application.Features.Login;

public record LoginResponse(string AccessToken, DateTime AccessTokenExpireDate, string RefreshToken, DateTime RefreshExpireDate);