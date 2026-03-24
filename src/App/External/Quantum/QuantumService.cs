using MyWebApi.App.Exceptions;

namespace MyWebApi.App.External.Quantum;

public class QuantumService(IQuantumClient client) : IQuantumService
{

    private readonly string ApiName = "Quantum Random Numbers";
    private readonly IQuantumClient _client = client;


    public async Task<QuantumResponseDto?> GetRandomSeedAsync()
    {
        return await _client.GetRandomSeedAsync() 
        ?? throw new NotFoundException($"No data returned from {ApiName} API");
    }
        
}

