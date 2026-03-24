namespace MyWebApi.App.DTO;

public class CreatedSecretDto
{
    public Guid Id { get; set; }
    public string AccessLink {get; set;} = string.Empty;
    public bool RequiresPassword { get; set; }
    
    public bool HasMaxViews => MaxViews >= 1;
    public int? MaxViews { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }



}