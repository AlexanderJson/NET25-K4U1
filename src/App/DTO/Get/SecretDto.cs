namespace MyWebApi.App.DTO;

public class SecretDto
{
    public string DecryptedContent { get; set; } = string.Empty;
    public int CurrentViews { get; set; }
    public int MaxViews { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

}