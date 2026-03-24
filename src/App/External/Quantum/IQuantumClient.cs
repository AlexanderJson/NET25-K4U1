

namespace MyWebApi.App.External.Quantum;


public interface IQuantumClient
{
    Task<QuantumResponseDto?> GetRandomSeedAsync();
}