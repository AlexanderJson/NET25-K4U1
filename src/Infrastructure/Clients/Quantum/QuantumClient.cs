using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MyWebApi.App.External.Quantum;
using MyWebApi.App.Options;
namespace Infrastructure.Clients.Quantum;
public class QuantumClient(HttpClient http, IOptions<QuantumOptions> options) : IQuantumClient
{
    private readonly HttpClient _http = http;
    private readonly QuantumOptions _options = options.Value;
    private readonly string headerKey = "x-api-key";
    public async Task<QuantumResponseDto?> GetRandomSeedAsync()
    {
        try
        {
            var req = new HttpRequestMessage
            (
                HttpMethod.Get,
                "?length=5&type=uint16&size=1" //TODO
            );
            req.Headers.Add(headerKey, _options.ApiKey);
            
            var resp = await _http.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<QuantumResponseDto>();
        }
        catch (Exception e) //TODO
        {
            throw new Exception($"Could not call Quantum API. Error message: {e.Message}, Full error: {e}");
        }
    }
}