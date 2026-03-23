using MyWebApi.Domain.Entities;

public class Secret
{
    public Guid Id {get; set;}
    public byte[] AccessLink{get; set;} = [];
    public string EncryptedContent {get; set;} = string.Empty;
    public int MaxViews { get; set; }
    public int CurrentViews { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string EncryptedPassword{get; set;} = string.Empty;
}