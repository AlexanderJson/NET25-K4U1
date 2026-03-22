

namespace MyWebApi.App.External.Nasa;


public interface ISentryService
{
      Task<SentryResponseDto?> GetAsteroidsAsync();
}