
namespace MyWebApi.App.External.Quantum;


public interface IQuantumService
{
    Task<QuantumResponseDto?> GetRandomSeedAsync();
}