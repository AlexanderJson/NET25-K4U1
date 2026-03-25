using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuth _auth;

    public AuthController(IAuth auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _auth.Login(dto.Username, dto.Password);
        
        return Ok(new { accessToken = token });
    }
    
    
}