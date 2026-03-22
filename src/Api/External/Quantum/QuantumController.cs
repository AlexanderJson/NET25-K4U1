using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.External.Quantum;


namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuantumController(IQuantumService service) : ControllerBase
{
    private readonly IQuantumService _service = service;

    [HttpGet]
    public async Task<ActionResult<QuantumResponseDto>> Get()
    {
        var response = await _service.GetRandomSeedAsync();
        return Ok(response);
    }
}