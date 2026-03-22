using MyWebApi.
namespace MyWebApi.App.Interfaces;

public class SentryService : ISentry
{
    private readonly SentryClient _client;
    public async Task<SentryResponseDto> GetAsteroids()
    {
        return await _client.
    }
}