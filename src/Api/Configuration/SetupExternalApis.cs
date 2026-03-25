using Infrastructure.Clients.Quantum;
using MyWebApi.App.External.Quantum;

namespace MyWebApi.Api.Configuration;

public static class SetupExternalApis
{
    public static IServiceCollection AddExternalApi
    (this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IQuantumService, QuantumService>();

        services.AddHttpClient<IQuantumClient, QuantumClient>(client =>
        {
            var baseUrl = config["ExternalApis:QuantumNumbers:BaseUrl"]
                ?? throw new InvalidOperationException("Missing configuration: ExternalApis:QuantumNumbers:BaseUrl");

            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config["Redis:ConnectionString"]
                ?? throw new InvalidOperationException("Missing configuration: Redis:ConnectionString");

            options.InstanceName = "MyWebApi:";
        });

        return services;
    }
}