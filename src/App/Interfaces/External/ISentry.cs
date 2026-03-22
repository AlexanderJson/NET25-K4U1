namespace MyWebApi.App.Interfaces;
public interface ISentry
{
    Task<SentryResponseDto> GetAsteroids();
}