using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitesController : ControllerBase
{
    private readonly IService<CreateInviteDto, InviteDto> _service;

    public InvitesController(IService<CreateInviteDto, InviteDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PagedResult<InviteDto>> GetInvites([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = _service.GetPaged(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<InviteDto> GetById(Guid id)
    {
        InviteDto invite = _service.GetById(id);
        return Ok(invite);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateInviteDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<InviteDto> Update(Guid id, [FromBody] CreateInviteDto dto)
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