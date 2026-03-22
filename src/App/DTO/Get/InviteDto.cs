namespace MyWebApi.App.DTO;

public class InviteDto
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }
}