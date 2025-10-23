namespace Identity.Domain.Services;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
    bool Verify(string refreshToken, string hashedRefreshToken);
}
