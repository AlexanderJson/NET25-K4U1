using System.ComponentModel.DataAnnotations;

namespace MyWebApi.App.DTO;

public class CreateDocumentDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Guid WorkspaceId { get; set; }

    [Required]
    public Guid CreatedById { get; set; }
}