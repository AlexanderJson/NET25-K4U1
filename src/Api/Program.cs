using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Repositories;

namespace MyWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IService<UserDto>, UserService>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}