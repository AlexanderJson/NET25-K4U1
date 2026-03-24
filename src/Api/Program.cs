// TODO - ta bort Program.cs från tools sen, kolla upp program.cs och regler där
// TODO läs perfekt kontroller + perfekt impl i build + response hanterign
// TODO vtikgit : fixa namespaces
// TODO + kolla .https filerna

using Infrastructure.Clients.Quantum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.Demo;
using MyWebApi.App.DTO;
using MyWebApi.App.External.Quantum;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Jwt.interfaces;
using MyWebApi.App.Options;
using MyWebApi.App.Services;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Infrastructure.Repositories.Users;
using Scalar.AspNetCore;
using System.Text;
using MyWebApi.App.Demo;
using Microsoft.AspNetCore.RateLimiting;

namespace MyWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        if(builder.Environment.IsDevelopment())
        {
            var jwtKey = builder.Configuration["Jwt:Key"];
            var secretKey = builder.Configuration["SecretEncryption:Key"];
            if(string.IsNullOrWhiteSpace(jwtKey))
            {
                var generate = DemoKeyGen.GenerateBase64Key(32);
                builder.Configuration["Jwt:Key"] = generate;
                Console.WriteLine($"IN DemO MODE!! Generated JWT Key: {generate}");
            }

            if(string.IsNullOrWhiteSpace(secretKey))
            {
                var generate = DemoKeyGen.GenerateBase64Key(32);
                builder.Configuration["SecretEncryption:Key"] = generate;
                Console.WriteLine($"[DEV] Generated Secret Key: {generate}");
            }
        }
        // Registers all controllers
        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Demo", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:5173",
                            "https://localhost:3000",
                            "https://localhost:5173"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

        // OpenAPI / Swagger
        builder.Services.AddOpenApi();

        /// Configuration bindigs
        
        // bind metadata from appsettings to the JwTOptions
        builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetSection("Jwt"));
        // same with external api
        builder.Services.Configure<QuantumOptions>(builder.Configuration.GetSection("ExternalApis:QuantumNumbers"));
        // secret key for secrets
        builder.Services.Configure<SecretOptions>(
            builder.Configuration.GetSection("SecretEncryption"));


        /// JwT setup 

        // Tries to read the JwT metadata (from JwTOptions) or throw error to user
        JwtOptions jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
                  ?? throw new InvalidOperationException("Jwt config missing");
        // enables our JwT auth
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {   
                        var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);
                        options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate that token issuer matches from options
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,
                        // Validates token audience (recipients of JwT)
                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,
                        // Here I use the same key used on sign-in to ensure the signature is valid
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        // Validates token is not expired
                        ValidateLifetime = true,
                        /* Currently = 0 , but if JwT was issued from external service, 
                        we could add a "grace period" to account for minimal time differences between this system and issuer system
                        */
                        ClockSkew = TimeSpan.FromSeconds(jwt.ClockSkewSeconds)
                    };
                });
        
        // Enables [requirement] flags
        builder.Services.AddAuthorization();
        // enables access to HttpContext for services. (I use it in UserContext)
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddRateLimiter(options =>
        {
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
        });

        /// Service registrations
        
        // authentication service + jwt
        builder.Services.AddScoped<IJwTService, JwTService>();
        builder.Services.AddScoped<IAuth, AuthService>();
        builder.Services.AddScoped<JwtGenerator>();
        // Domain services
        builder.Services.AddScoped<ICrudService<CreateUserDto, UserDto>, UserService>();
        builder.Services.AddScoped<ISecretService, SecretService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserService<CreateUserDto, UserDto>,UserService>();
        builder.Services.AddScoped<UserContext>(); 
        /// Repository registration
        
        // Domain Repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISecretRepository, SecretRepository>();

        
        /// External APIs 
        /// (kept in its own section since additolal apis might be added)
        
        /// QUANTUM API
        // Service
        builder.Services.AddScoped<IQuantumService, QuantumService>();
        // HttpClient
        builder.Services.AddHttpClient<IQuantumClient, QuantumClient>(client =>
        {
            var quantumBaseUrl = builder.Configuration["ExternalApis:QuantumNumbers:BaseUrl"]
                ?? throw new InvalidOperationException("Missing configuration: ExternalApis:QuantumNumbers:BaseUrl");

            client.BaseAddress = new Uri(quantumBaseUrl);
        });
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration["Redis:ConnectionString"]
                ?? throw new InvalidOperationException("Missing configuration: Redis:ConnectionString");

            options.InstanceName = "MyWebApi:";
        });


        /// DATABASE CONFIG
   
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Missing connection string: Default"))
            );
        
        

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Seed.SeedAll(db);
        }
        

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.WithTitle("DEV API ")
                       .WithTheme(ScalarTheme.Moon);
            });
        }
        app.UseHttpsRedirection();
        app.UseCors("Demo");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();
        app.Run();
}


}