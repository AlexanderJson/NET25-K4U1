using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.External.Nasa;


namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SentryController(ISentryService service) : ControllerBase
{
    private readonly ISentryService _service = service;

    [HttpGet]
    public async Task<ActionResult<SentryResponseDto>> Get()
    {
        var response = await _service.GetAsteroidsAsync();
        return Ok(response);
    }
}