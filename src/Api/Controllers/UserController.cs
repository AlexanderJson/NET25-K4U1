using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService<CreateUserDto, UserDto> service, UserContext userContext) : ControllerBase
{
    private readonly IUserService<CreateUserDto, UserDto> _service = service;
    private readonly UserContext _userContext = userContext;

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _service.GetById(id);
        return Ok(user);
    }

    [EnableRateLimiting("daily-limit")]
    [HttpPost("generate-password")]
    public async Task<IActionResult> GeneratePassword()
    {
        var userId = _userContext.UserId;

        if (userId is null) return Unauthorized();

        var password = await _service.GeneratePassword();
        await _service.SetPassword(userId.Value, password);

        return Ok(new
        {
            Message = "Password generated and updated",
            Password = password
        });
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        var userId = _userContext.UserId;

        if (userId is null) return Unauthorized();

        var user = await _service.GetById(userId.Value);
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPaged(page, pageSize);
        return Ok(result);
    }

    [HttpGet("username/search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string username)
    {
        var result = await _service.SearchByUsername(username);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        await _service.Add(dto);
        return Created("", null);
    }

    [Authorize]
    [EnableRateLimiting("daily-limit")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] CreateUserDto dto)
    {
        var userId = _userContext.UserId;
        if (userId != id) return Forbid();

        var updated = await _service.Update(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = _userContext.UserId;
        if (userId != id) return Forbid();

        await _service.Delete(id);
        return NoContent();
    }
}