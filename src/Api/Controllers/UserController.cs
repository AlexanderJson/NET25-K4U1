using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ICrudService<CreateUserDto, UserDto> _service;

    public UsersController(ICrudService<CreateUserDto, UserDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PagedResult<UserDto>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = _service.GetPaged(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<UserDto> GetById(Guid id)
    {
        UserDto user = _service.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<UserDto> Update(Guid id, [FromBody] CreateUserDto dto)
    {
        var updated = _service.Update(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _service.Delete(id);
        return NoContent();
    }
}