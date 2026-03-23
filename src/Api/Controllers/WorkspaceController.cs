using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

namespace MyWebApi.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly ICrudService<CreateWorkspaceDto, WorkspaceDto> _service;

    public WorkspacesController(ICrudService<CreateWorkspaceDto, WorkspaceDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PagedResult<WorkspaceDto>> GetWorkspaces([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = _service.GetPaged(page, pageSize);
        return Ok(result);
    }



    [HttpGet("{id:guid}")]
    public ActionResult<WorkspaceDto> GetById(Guid id)
    {
        WorkspaceDto workspace = _service.GetById(id);
        return Ok(workspace);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateWorkspaceDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<WorkspaceDto> Update(Guid id, [FromBody] CreateWorkspaceDto dto)
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