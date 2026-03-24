using System.Security.Cryptography;
using System.Text;

namespace MyWebApi.App.Services;

public interface ITokenService
{
    public string GenerateToken();
    public byte[] HashToken(string token);
}

public class TokenService : ITokenService
{

    public  string GenerateToken()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes);
    }

    public  byte[] HashToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        return SHA256.HashData(tokenBytes);
    }
}