using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyWebApi.App.External.Quantum;


namespace MyWebApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuantumRandomsController(IQuantumService service) : ControllerBase
{
    private readonly IQuantumService _service = service;

    [EnableRateLimiting("strict")]
    [HttpGet]
    public async Task<ActionResult<QuantumResponseDto>> Get()
    {
        var result = await _service.GetRandomSeedAsync();
        if (result == null)
            return StatusCode(502, "No response from Quantum API.");

        return Ok(result);
    }
}