using System.ComponentModel.DataAnnotations;

namespace MyWebApi.App.DTO;

public class CreateUserDto
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = "";
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";
    [Required]
    public string Name { get; set; } = "";

}