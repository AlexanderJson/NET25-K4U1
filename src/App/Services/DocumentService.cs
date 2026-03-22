using MyWebApi.Api.Interfaces;
using MyWebApi.App.Abstracts;
using MyWebApi.App.DTO;

namespace MyWebApi.App.Services;

public class DocumentService(IDocumentRepository documentRepo)
    : AService<CreateDocumentDto, DocumentDto, Document>(documentRepo)
{
    protected override Document GetEntity(CreateDocumentDto dto)
    {
        return new Document
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            WorkspaceId = dto.WorkspaceId,
            CreatedById = dto.CreatedById,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    protected override DocumentDto ReturnDto(Document entity)
    {
        return new DocumentDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            WorkspaceId = entity.WorkspaceId,
            CreatedById = entity.CreatedById,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    protected override void ApplyUpdate(Document entity, CreateDocumentDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title))
            entity.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Content))
            entity.Content = dto.Content;

        if (dto.WorkspaceId != Guid.Empty)
            entity.WorkspaceId = dto.WorkspaceId;

        if (dto.CreatedById != Guid.Empty)
            entity.CreatedById = dto.CreatedById;

        entity.UpdatedAt = DateTime.UtcNow;
    }

    protected override void ValidateArgs(CreateDocumentDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Document title is required.");

        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new ArgumentException("Document content is required.");

        if (dto.WorkspaceId == Guid.Empty)
            throw new ArgumentException("WorkspaceId is required.");

        if (dto.CreatedById == Guid.Empty)
            throw new ArgumentException("CreatedById is required.");
    }

    protected override IQueryable<Document> ApplyOrdering(IQueryable<Document> query)
    {
        return query.OrderBy(d => d.Title);
    }
}