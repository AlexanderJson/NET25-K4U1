using MyWebApi.App.Options;
public class RateLimitOptions
{
    public int PermitLimit { get; set; }
    public int WindowDays { get; set; }
}