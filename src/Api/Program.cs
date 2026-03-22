// TODO - ta bort Program.cs från tools sen, kolla upp program.cs och regler där
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Infrastructure.Repositories;
using Scalar.AspNetCore;  

namespace MyWebApi;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi(); 

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IService<CreateUserDto,UserDto>, UserService>();
        
        private readonly string sentryUri = "";

        builder.Services.AddHttpClient<>(client =>
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