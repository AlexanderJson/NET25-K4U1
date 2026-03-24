using MyWebApi.Domain.Entities;

public class Secret
{
    public Guid Id {get; set;}
    /// <summary>
    /// Hash of the public access token,
    /// so this is stored instead of a raw token for obvious reasons
    /// </summary>
    public byte[] HashedAccessToken{get; set;} = [];

    /// <summary>
    /// AES-GCM nonce used to encrypt,
    /// but I store this as a Base64 text since for simplicity sake!
    /// </summary>
    public string Nonce {get; set;} = string.Empty;
    
    /// <summary>
    /// Encrypted content stored as Base64 text
    /// </summary>
    public string CipherText {get; set;} = string.Empty;


    public string Tag {get; set;} = string.Empty;


    /// <summary>
    /// Optional. Lets the secret be opened N amount of times.
    /// I use atomic race conditioning to make sure this works as good as possible
    /// </summary>
    public int? MaxViews {get; set;}

    /// <summary>
    /// General metadata + used for MaxViews
    /// </summary>
    public int CurrentViews {get; set;}

    public int ViewsLeft => (int)(MaxViews - CurrentViews);

    /// <summary>
    /// When it expires
    /// </summary>
    public DateTime ExpiresAt {get;set;}
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Just soft deletion. 
    /// </summary>
    public bool? ShouldDelete {get; set;}
    public DateTime? DeletedAt {get; set;}

    /// <summary>
    /// This is more for later, if I want
    /// to include logic for long-term secrets etc.
    /// </summary>
    public Guid? Owner{get; set;}
    public List<Guid>? Permissions {get; set;}

    /// <summary>
    /// This is just an optional password client can set
    /// </summary>
    public string? PasswordHash {get;set;}
    public string? PasswordSalt {get; set;}

}