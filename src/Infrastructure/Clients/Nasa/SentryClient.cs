using System.Net.Http.Json;
using MyWebApi.App.External.Nasa;
namespace Infrastructure.Clients.Nasa;
public class SentryClient : ISentryClient
{
    private readonly HttpClient _http;

    public SentryClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<SentryResponseDto?> GetAsteroidsAsync()
    {
        return await _http.GetFromJsonAsync<SentryResponseDto>("sentry.api");
    }


}