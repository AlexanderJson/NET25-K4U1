using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
public class JwtGenerator(IOptions<JwtOptions> options) : IJwtGenerator
{
    private readonly JwtOptions _options = options.Value;
    public string Generate(IEnumerable<Claim> claims)
    {   
        // Chosen signing algorithm. Its symmetrical 
        // but safe enough for this REST API. 
        var algo = SecurityAlgorithms.HmacSha512;
        var bytes = Encoding.UTF8.GetBytes(_options.Key);
        // I wrap the key in a SymmetricSecurityKey wrapper
        // because we are performing symmetric signing 
        var key = new SymmetricSecurityKey(bytes);
        var creds = new SigningCredentials
        (
            key,
            algo
        );
        // Token is built with the header+payload
        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpireMinutes),
            signingCredentials: creds
        );
        // then we seralise it
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}