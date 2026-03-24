using MyWebApi.App.Exceptions;

namespace MyWebApi.App.External.Nasa;

public class SentryService(ISentryClient client) : ISentryService
{

    private readonly string ApiName = "NASA Sentry";
    private readonly ISentryClient _sentryClient = client;
    

    public async Task<SentryResponseDto?> GetAsteroidsAsync()
    {
        return await _sentryClient.GetAsteroidsAsync() 
        ?? throw new NotFoundException($"No data returned from {ApiName} API");
    }
}

