using Microsoft.AspNetCore.Mvc;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly ICrudService<CreateDocumentDto, DocumentDto> _service;

    public DocumentsController(ICrudService<CreateDocumentDto, DocumentDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PagedResult<DocumentDto>> GetDocuments([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = _service.GetPaged(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<DocumentDto> GetById(Guid id)
    {
        DocumentDto document = _service.GetById(id);
        return Ok(document);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateDocumentDto dto)
    {
        _service.Add(dto);
        return Created("", null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<DocumentDto> Update(Guid id, [FromBody] CreateDocumentDto dto)
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