using MyWebApi.Domain.Enums;

namespace MyWebApi.Domain.Entities;

public class WorkSpaceMember
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public WorkspaceRole Role { get; set; } = 0;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}