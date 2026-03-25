using Microsoft.AspNetCore.RateLimiting;
using MyWebApi.App.Options;
using System.Threading.RateLimiting;

namespace MyWebApi.Api.Configuration;

public static class SetupRateLimits
{
    public static IServiceCollection AddRateLimits(
        this IServiceCollection services,
        IConfiguration config)
    {
        var rateLimit = config
            .GetSection("RateLimiting:GeneratePassword")
            .Get<RateLimitOptions>();


        services.AddRateLimiter(options =>
        {
            options.AddPolicy("daily-limit", opt =>
            {
                var userId = opt.User?.FindFirst("sub")?.Value
                    ?? opt.Connection.RemoteIpAddress?.ToString()
                    ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: userId,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimit!.PermitLimit,
                        Window = TimeSpan.FromDays(rateLimit.WindowDays),
                        QueueLimit = 0
                    });
            });

            options.AddFixedWindowLimiter("api", opt =>
            {
                opt.PermitLimit = 20;
                opt.Window = TimeSpan.FromSeconds(10);
                opt.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter("strict", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromSeconds(10);
                opt.QueueLimit = 0;
            });

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    AutoReplenishment = true
                }));
        });

        return services;
    }
}