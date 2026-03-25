// TODO vtikgit : fixa namespaces

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyWebApi.Api.Configuration;

public static class SetupJwt
{
    public static IServiceCollection AddJwTValidation(this IServiceCollection services, IConfiguration config)
    {
        // bind metadata from appsettings to the JwTOptions
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        // Tries to read the JwT metadata (from JwTOptions) or throw error to user
        var jwt = config.GetSection("Jwt").Get<JwtOptions>()
                  ?? throw new InvalidOperationException("Jwt config missing");

        // enables our JwT auth
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.FromSeconds(jwt.ClockSkewSeconds)
                };
            });

        services.AddAuthorization();

        return services;
    }
}