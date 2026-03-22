using MyWebApi.Domain.Entities;

public class Document
{
    public Guid Id {get; set;}
    public string Title { get; set; } = string.Empty;
    public string Content {get; set;} = string.Empty;
    
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}