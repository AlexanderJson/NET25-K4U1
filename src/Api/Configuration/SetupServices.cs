using Microsoft.Extensions.DependencyInjection;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Jwt.interfaces;
using MyWebApi.App.Services;

using MyWebApi.Infrastructure.Repositories.Users;

namespace MyWebApi.Api.Configuration;

public static class SetupServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        /// Service registrations

        // authentication service + jwt
        services.AddScoped<IJwTService, JwTService>();
        services.AddScoped<IAuth, AuthService>();
        services.AddScoped<JwtGenerator>();

        // Domain services
        services.AddScoped<ICrudService<CreateUserDto, UserDto>, UserService>();
        services.AddScoped<ISecretService, SecretService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService<CreateUserDto, UserDto>, UserService>();

        services.AddScoped<UserContext>();

        /// Repository registration

        // Domain Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISecretRepository, SecretRepository>();

        return services;
    }
}