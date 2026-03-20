using Microsoft.EntityFrameworkCore;
using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Infrastructure.Repositories;
using MyWebApi.Api.Interfaces;
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
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IService<CreateUserDto,UserDto>, UserService>();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));
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