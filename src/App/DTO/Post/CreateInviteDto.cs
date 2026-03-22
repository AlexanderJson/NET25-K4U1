using System.ComponentModel.DataAnnotations;

namespace MyWebApi.App.DTO;

public class CreateInviteDto
{
    [Required]
    public Guid WorkspaceId { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "Viewer";
}