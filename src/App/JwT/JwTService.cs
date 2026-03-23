using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyWebApi.App.Jwt.interfaces;

public class JwTService(JwtGenerator generator) : IJwTService
{
    private readonly JwtGenerator _generator = generator;
    public string GenerateToken(Guid userId, string username)
    {
        var claims = new List<Claim>
        {
            // Subject claim aka who the authenticated identity is
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        return _generator.Generate(claims);
    }
}