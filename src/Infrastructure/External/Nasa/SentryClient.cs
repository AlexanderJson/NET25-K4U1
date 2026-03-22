using System.Net.Http.Json;

namespace MyWebApi.Infrastructure.External;

public class SentryClient(HttpClient http)
{
    private readonly HttpClient _http = http;
    public async Task<SentryResponseDto> GetAsteroidData()
    {
        return await _http.GetFromJsonAsync<SentryResponseDto>("sentry.api");
    }
    

}