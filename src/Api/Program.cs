// TODO - ta bort Program.cs från tools sen, kolla upp program.cs och regler där
// TODO läs perfekt kontroller + perfekt impl i build + response hanterign
// TODO vtikgit : fixa namespaces
// TODO + kolla .https filerna

using Infrastructure.Clients.Nasa;
using Infrastructure.Clients.Quantum;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
using MyWebApi.App.External.Nasa;
using MyWebApi.App.External.Quantum;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Options;
using MyWebApi.App.Services;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Infrastructure.Repositories;
using MyWebApi.Infrastructure.Repositories.Users;
using Scalar.AspNetCore;

namespace MyWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<IInviteRepository, InviteRepository>();

        // Services
        builder.Services.AddScoped<IService<CreateUserDto, UserDto>, UserService>();
        builder.Services.AddScoped<IService<CreateWorkspaceDto, WorkspaceDto>, WorkspaceService>();
        builder.Services.AddScoped<IService<CreateDocumentDto, DocumentDto>, DocumentService>();
        builder.Services.AddScoped<IService<CreateInviteDto, InviteDto>, InviteService>();

        builder.Services.Configure<QuantumOptions>(
            builder.Configuration.GetSection("ExternalApis:QuantumNumbers"));

        builder.Services.AddHttpClient<IQuantumClient, QuantumClient>(client =>
        {
            var quantumBaseUrl = builder.Configuration["ExternalApis:QuantumNumbers:BaseUrl"]
                ?? throw new InvalidOperationException("Missing configuration: ExternalApis:QuantumNumbers:BaseUrl");

            client.BaseAddress = new Uri(quantumBaseUrl);
        });

        builder.Services.AddScoped<IQuantumService, QuantumService>();

        builder.Services.AddScoped<ISentryService, SentryService>();
        builder.Services.AddHttpClient<ISentryClient, SentryClient>(client =>
        {
            client.BaseAddress = new Uri("https://ssd-api.jpl.nasa.gov/");
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Seed.SeedAll(db);        
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.WithTitle("My API")
                       .WithTheme(ScalarTheme.Moon);
            });
        }

        app.MapControllers();
        app.Run();
    }
}
