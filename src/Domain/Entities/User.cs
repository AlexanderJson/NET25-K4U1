

namespace MyWebApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username {get; set;} = "";
    public string HashedPassword {get; set;} = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Workspace> OwnedWorkspaces { get; set; } = new List<Workspace>();

    public ICollection<WorkSpaceMember> WorkspaceMemberships { get; set; } = new List<WorkSpaceMember>();

    public ICollection<Document> CreatedDocuments { get; set; } = new List<Document>();

}