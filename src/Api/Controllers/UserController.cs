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
    public IActionResult Create(CreateUserDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }
}