namespace MyWebApi.App.DTO;

public class SecretDto
{
    public Guid SecretId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int? MaxViews { get; set; }
    public int ViewCount { get; set; }

}