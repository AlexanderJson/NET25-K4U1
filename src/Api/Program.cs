// TODO - ta bort Program.cs från tools sen, kolla upp program.cs och regler där

using Infrastructure.Clients.Nasa;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
using MyWebApi.App.External.Nasa;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;
using MyWebApi.Infrastructure.Data;
using MyWebApi.App.Options;
using MyWebApi.Infrastructure.Repositories.Users;
using Scalar.AspNetCore;
using MyWebApi.App.External.Quantum;
using Infrastructure.Clients.Quantum;

namespace MyWebApi;

// TODO läs perfekt kontroller + perfekt impl i build + response hanterign
// TODO vtikgit : fixa namespaces

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<QuantumOptions>(
            builder.Configuration.GetSection("ExternalApis:QuantumNumbers"));
        builder.Services.AddHttpClient<IQuantumClient, QuantumClient>(client =>
        {
             // TODO + kolla .https filerna
            client.BaseAddress = new Uri("https://api.quantumnumbers.anu.edu.au");
        });

        builder.Services.AddScoped<IQuantumService, QuantumService>();


        builder.Services.AddControllers();
        builder.Services.AddOpenApi(); 
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IService<CreateUserDto,UserDto>, UserService>();
        
        string sentryUri = "https://ssd-api.jpl.nasa.gov/";

        builder.Services.AddScoped<ISentryService, SentryService>();
        builder.Services.AddHttpClient<ISentryClient, SentryClient>(client =>
        {
            client.BaseAddress = new Uri(sentryUri);
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            Seed.SeedUsers(db);
        }
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi(); 

            app.MapScalarApiReference(options => {
                options.WithTitle("My API")
                       .WithTheme(ScalarTheme.Moon);
            });
        }

        app.MapControllers();
        app.Run();
    }
}