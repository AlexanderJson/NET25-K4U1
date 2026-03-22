using System.Text.Json.Serialization;

namespace MyWebApi.App.External.Quantum;

public class QuantumResponseDto
{
    [JsonPropertyName("data")]
    public ushort[] Data { get; set; } = Array.Empty<ushort>();
   
}

