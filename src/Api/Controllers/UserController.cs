using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<CreateUserDto, UserDto> _service;
    public UserController(IService<CreateUserDto, UserDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _service.GetAll();
        return Ok(users);
    }

    [HttpPost]
    public IActionResult Create([FromBody]CreateUserDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var user = _service.getById(id);
        return Ok(user);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] CreateUserDto dto)
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