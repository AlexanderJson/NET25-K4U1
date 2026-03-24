namespace MyWebApi.App.DTO;

public class SecretMetadataDto
{
    public bool RequiresPassword { get; set; }
    public int RemainingViews { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
}