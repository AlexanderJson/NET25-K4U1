using Microsoft.AspNetCore.Mvc;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecretsController(ISecretService secretService) : ControllerBase
{
    private readonly ISecretService _secretService = secretService;

    [HttpPost]
    public ActionResult<CreatedSecretDto> Create(CreateSecretDto dto)
    {
        var result =  _secretService.CreateSecret(dto);

        return CreatedAtAction(
            nameof(Get),
            new { accessToken = result.AccessToken },
            result);
    }

    [HttpGet("{accessToken}")]
    public  ActionResult<SecretDto> Get(string accessToken)
    {
        var result =  _secretService.GetByToken(accessToken);
        return Ok(result);
    }
}