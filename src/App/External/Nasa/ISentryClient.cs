
namespace MyWebApi.App.External.Nasa;


public interface ISentryClient
{
    Task<SentryResponseDto?> GetAsteroidsAsync();
}