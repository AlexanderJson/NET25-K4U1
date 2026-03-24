using System.Text.Json.Serialization;

namespace MyWebApi.App.External.Nasa;

public class SentryObjectDto
{
    [JsonPropertyName("des")]
    public string? Des {get; set;}
    [JsonPropertyName("name")]
    public string? Name {get; set;}
    [JsonPropertyName("ip")]
    public string? Ip {get; set;}
}
public class SentryResponseDto
{
    public List<SentryObjectDto>? Data {get; set;}
}
