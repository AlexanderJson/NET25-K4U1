using System.ComponentModel.DataAnnotations;

public class CreateSecretDto
{
    [Required]
    [StringLength(10_000, MinimumLength = 1)]
    public string Content{get; set;} = string.Empty;

    [MinLength(50)]
    [MaxLength(200)]
    public string? Password { get; set; }
    
    [Range(1, 100)]
    public int MaxViews { get; set; } = 1;

    [Range(1, 43_200)]  
    public int ExpiresInMinutes { get; set; } = 60;
}