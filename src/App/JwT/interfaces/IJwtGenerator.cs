using System.Security.Claims;
/// <summary>
/// Here we perform the logic to generate actual tokens,
/// responsible for SecurityKey etc. 
/// </summary>

public interface IJwtGenerator{string Generate(IEnumerable<Claim> claims);}
