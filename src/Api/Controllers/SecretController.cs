using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecretsController(ISecretService secretService, UserContext context) : ControllerBase
{
    private readonly ISecretService _secretService = secretService;
    private readonly UserContext _context = context;


    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUserSecrets()
    {
        var userId = _context.UserId;

        if (userId is null)
            return Unauthorized();

        var result = await _secretService.GetByUserId(userId.Value);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreatedSecretDto>> Create(CreateSecretDto dto)
    {
        var userId = _context.UserId;
        var result = await _secretService.CreateSecret(dto, userId);

        return CreatedAtAction(
            nameof(Get),
            new { accessToken = result.AccessToken },
            result);
    }

    [HttpGet("{accessToken}")]
    public async Task<ActionResult<SecretDto>> Get(string accessToken)
    {
        var result = await _secretService.GetByToken(accessToken);
        return Ok(result);
    }
}