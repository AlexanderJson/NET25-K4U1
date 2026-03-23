using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Services;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuth _auth;

    public AuthController(IAuth auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var token = _auth.Login(dto.Username, dto.Password);
        return Ok(new { accessToken = token });
    }
    
    
}