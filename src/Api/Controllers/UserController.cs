using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService<CreateUserDto, UserDto> service, UserContext userContext) : ControllerBase
{
    private readonly IUserService<CreateUserDto, UserDto> _service = service;
    private readonly UserContext _userContext = userContext;



    // later I would add roles here, to make sure only admin can check etc.
    // left it unsafe for demo atm
    [HttpGet("{id:guid}")]
    public ActionResult<UserDto> GetById(Guid id)
    {
        var userId = _userContext.UserId;
        UserDto user = _service.GetById(id);
        return Ok(user);
    }

    [EnableRateLimiting("strict")]
    [HttpPost("generate-password")]
    public async Task<IActionResult> GeneratePassword()
    {
        var userId = _userContext.UserId;

        if (userId is null)
            return Unauthorized();

        var password = await _service.GeneratePassword();
        _service.SetPassword(userId.Value, password);

        return Ok(new
        {
            Message = "Password generated and updated",
            Password = password
        });
    }

    [HttpGet("me")]
    public ActionResult<UserDto> Me()
    {
        var userId = _userContext.UserId;

        if (userId is null)
            return Unauthorized();

        var user = _service.GetById(userId.Value);
        return Ok(user);
    }

    [HttpGet]
    public ActionResult<PagedResult<UserDto>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        
        var result = _service.GetPaged(page, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public IActionResult SearchUsers([FromQuery] string username)
    {
        var result = _service.SearchByUsername(username);
        return Ok(result);
    }
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<UserDto> Update(Guid id, [FromBody] CreateUserDto dto)
    {
        var userId = _userContext.UserId;
        if(userId != id) return Forbid();
        var updated = _service.Update(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var userId = _userContext.UserId;
        if(userId != id) return Forbid();
        _service.Delete(id);
        return NoContent();
    }
}