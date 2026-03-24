// DTO might not be the "right" word for this, not sure
public class AadDto
{
    public Guid SecretId {get; set;}
    public DateTime ExpiresAt {get; set;}

    // if its 0 = no max views.
    public int MaxViews {get; set;} = 0;
    public bool RequiresPassword {get;set;}

    // temporary, will fix later
    public Guid OwnerId {get; set;}
}