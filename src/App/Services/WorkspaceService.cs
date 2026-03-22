using MyWebApi.Api.Interfaces;
using MyWebApi.App.Abstracts;
using MyWebApi.App.DTO;
using MyWebApi.Domain.Entities;

namespace MyWebApi.App.Services;

public class WorkspaceService(IWorkspaceRepository workspaceRepo)
    : AService<CreateWorkspaceDto, WorkspaceDto, Workspace>(workspaceRepo)
{
    protected override Workspace GetEntity(CreateWorkspaceDto dto)
    {
        return new Workspace
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId,
            CreatedAt = DateTime.UtcNow
        };
    }

    protected override WorkspaceDto ReturnDto(Workspace entity)
    {
        return new WorkspaceDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            OwnerId = entity.OwnerId,
            CreatedAt = entity.CreatedAt
        };
    }

    protected override void ApplyUpdate(Workspace entity, CreateWorkspaceDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name))
            entity.Name = dto.Name;

        entity.Description = dto.Description;

        if (dto.OwnerId != Guid.Empty)
            entity.OwnerId = dto.OwnerId;
    }

    protected override void ValidateArgs(CreateWorkspaceDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Workspace name is required.");

        if (dto.OwnerId == Guid.Empty)
            throw new ArgumentException("OwnerId is required.");
    }

    protected override IQueryable<Workspace> ApplyOrdering(IQueryable<Workspace> query)
    {
        return query.OrderBy(w => w.Name);
    }
}