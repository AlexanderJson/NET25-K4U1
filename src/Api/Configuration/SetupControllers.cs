
namespace MyWebApi.Api.Configuration;

public static class SetupControllers
{
    public static IServiceCollection AddAppConfig(this IServiceCollection services)
    {
        // Registers all controllers
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("ProdPolicy", policy =>
            {
                policy.WithOrigins("https://justafakeexampme.com")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });

            options.AddPolicy("DevPolicy", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:5173",
                        "https://localhost:3000",
                        "https://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}