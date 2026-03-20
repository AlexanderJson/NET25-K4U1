using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IService<UserDto> _service;
    public UserController(IService<UserDto> service)
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
    public IActionResult Create(UserDto dto)
    {
        _service.Add(dto);
        return Ok();
    }
}