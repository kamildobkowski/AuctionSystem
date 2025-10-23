using System.Security.Cryptography;
using System.Text;
using Identity.Domain.Services;

namespace Identity.Infrastructure.Token;

public sealed class RefreshTokenHasher 
	: IRefreshTokenHasher
{
	private const string Key = "sss";
	public string Hash(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
        
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var key = Encoding.UTF8.GetBytes(Key);
        var hash = HMACSHA256.HashData(key, bytes);
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string refreshToken, string hashedRefreshToken)
	{
		var computed = Hash(refreshToken);
		var a = Convert.FromBase64String(computed);
		var b = Convert.FromBase64String(hashedRefreshToken);
		return FixedTimeEquals(a, b);
	}

	private static bool FixedTimeEquals(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
	{
		if (a.Length != b.Length) return false;
		var diff = 0;
		for (var i = 0; i < a.Length; i++)
			diff |= a[i] ^ b[i];
		return diff == 0;
	}
}