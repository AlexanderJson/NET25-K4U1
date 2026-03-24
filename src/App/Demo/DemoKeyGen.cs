using System.Security.Cryptography;
namespace MyWebApi.App.Demo;
public static class DemoKeyGen
{
    public static string GenerateBase64Key(int bytes)
    {
        var key = new byte[bytes];
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }
}