using System.ComponentModel.DataAnnotations;

public class CreateSecretDto
{
    [Required]
    [StringLength(10_000, MinimumLength = 1)]
    public string Content{get; set;} = string.Empty;
    public int? MaxViews { get; set; } = 0;
    [Required]
    public DateTime ExpiresAt { get; set; }
    public string? Label {get; set;}


    /*
    [MinLength(50)]
    [MaxLength(200)]
    public string? Password { get; set; }
    */
}