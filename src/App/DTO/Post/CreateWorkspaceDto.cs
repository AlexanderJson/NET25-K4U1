using System.ComponentModel.DataAnnotations;

namespace MyWebApi.App.DTO;

public class CreateWorkspaceDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public Guid OwnerId { get; set; }
}