using MyWebApi.Domain.Entities;

public class Secret
{
    public Guid Id {get; set;}
    /// <summary>
    /// Hash of the public access token,
    /// </summary>
    public byte[] HashedAccessToken{get; set;} = [];


    
    /// <summary>
    /// Encrypted content stored as Base64 text
    /// </summary>
    public string EncryptedContent {get; set;} = string.Empty;

    /// <summary>
    /// Optional. Lets the secret be opened N amount of times.
    /// I use atomic race conditioning to make sure this works as good as possible
    /// </summary>
    public int? MaxViews {get; set;} =0;

    /// <summary>
    /// General metadata + used for MaxViews
    /// </summary>
    public int CurrentViews {get; set;}

    /// <summary>
    /// When it expires
    /// </summary>
    public DateTime ExpiresAt {get;set;}
    public DateTime CreatedAt { get; set; }
    public Guid? OwnerId { get; set; }
    public User? Owner { get; set; }    
    public bool RequiresPassword {get; set;}

/*
    /// <summary>
    /// This is just an optional password client can set
    /// </summary>
    public string? PasswordHash {get;set;}
    public string? PasswordSalt {get; set;}

    public List<Guid>? Permissions {get; set;}

*/
}