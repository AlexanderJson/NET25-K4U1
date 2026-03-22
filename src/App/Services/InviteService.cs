using MyWebApi.Api.Interfaces;
using MyWebApi.App.Abstracts;
using MyWebApi.App.DTO;
using MyWebApi.Domain.Entities;

namespace MyWebApi.App.Services;

public class InviteService(IInviteRepository inviteRepo)
    : AService<CreateInviteDto, InviteDto, Invite>(inviteRepo)
{
    private readonly IInviteRepository _inviteRepo = inviteRepo;

    protected override Invite GetEntity(CreateInviteDto dto)
    {
        return new Invite
        {
            Id = Guid.NewGuid(),
            WorkspaceId = dto.WorkspaceId,
            Email = dto.Email,
            Role = dto.Role,
            Token = CreateUniqueToken(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false
        };
    }

    protected override InviteDto ReturnDto(Invite entity)
    {
        return new InviteDto
        {
            Id = entity.Id,
            WorkspaceId = entity.WorkspaceId,
            Email = entity.Email,
            Role = entity.Role,
            Token = entity.Token,
            ExpiresAt = entity.ExpiresAt,
            IsUsed = entity.IsUsed,
            CreatedAt = entity.CreatedAt
        };
    }

    protected override void ApplyUpdate(Invite entity, CreateInviteDto dto)
    {
        if (dto.WorkspaceId != Guid.Empty)
            entity.WorkspaceId = dto.WorkspaceId;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            entity.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Role))
            entity.Role = dto.Role;
    }

    protected override void ValidateArgs(CreateInviteDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");

        if (dto.WorkspaceId == Guid.Empty)
            throw new ArgumentException("WorkspaceId is required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Role))
            throw new ArgumentException("Role is required.");
    }

    protected override IQueryable<Invite> ApplyOrdering(IQueryable<Invite> query)
    {
        return query.OrderByDescending(i => i.CreatedAt);
    }

    private string CreateUniqueToken()
    {
        string token;

        do
        {
            token = Guid.NewGuid().ToString("N");
        }
        while (_inviteRepo.TokenExists(token));

        return token;
    }
}