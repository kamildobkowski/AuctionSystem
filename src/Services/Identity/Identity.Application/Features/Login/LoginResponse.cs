namespace Identity.Application.Features.Login;

public record LoginResponse(string AccessToken, string RefreshToken);