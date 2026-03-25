using Microsoft.EntityFrameworkCore;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Api.Configuration;

public static class SetupDb
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                config.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Missing connection string: Default"))
        );

        return services;
    }
}