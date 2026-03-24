namespace MyWebApi.App.DTO;

/// <summary>
/// returned when creating secret initially
/// </summary>
public class CreatedSecretDto
{
    public Guid Id { get; set; }
    public string AccessToken {get; set;} = string.Empty;
    public int? MaxViews { get; set; }
    public DateTime ExpiresAt { get; set; }

}