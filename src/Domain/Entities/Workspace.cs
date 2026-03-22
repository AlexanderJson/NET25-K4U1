namespace MyWebApi.Domain.Entities;

public class Workspace
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid OwnerId { get; set; }

    public User? Owner { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<WorkSpaceMember> Members { get; set; } = new List<WorkSpaceMember>();

    public ICollection<Invite> Invites { get; set; } = new List<Invite>();
}
