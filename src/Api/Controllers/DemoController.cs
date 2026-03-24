using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/dev")]
public class DevController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    [HttpPost("seed-runtime")]
    public IActionResult SeedRuntime()
    {
        Console.WriteLine("[DEV] Runtime seed started.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = $"DemoUser_{Guid.NewGuid().ToString()[..6]}",
            Email = $"demo_{Guid.NewGuid().ToString()[..6]}@test.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456")
        };

        _db.Users.Add(user);

        var rawToken = $"runtime-token-{Guid.NewGuid()}";

        var secret = new Secret
        {
            Id = Guid.NewGuid(),
            OwnerId = user.Id,
            EncryptedContent = "runtime-demo-encrypted",
            HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            MaxViews = 1,
            CurrentViews = 0,
            RequiresPassword = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.Secrets.Add(secret);
        _db.SaveChanges();
        return Ok(new
        {
            User = new
            {
                user.Id,
                user.Username,
                user.Email,
                Password = "123456"
            },
            Secret = new
            {
                secret.Id,
                RawAccessToken = rawToken,
                secret.ExpiresAt
            }
        });
    }

    [HttpPost("seed")]
    public IActionResult Seed()
    {
        var users = new List<User>();

        for (int i = 0; i < 10; i++)
        {   
            Console.Write($"AT: {i}");
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = $"DemoUser_{Guid.NewGuid().ToString()[..6]}",
                Email = $"demo_{Guid.NewGuid().ToString()[..6]}@test.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456")
            });
        }

        _db.Users.AddRange(users);
        _db.SaveChanges();

        return Ok($"Inserted {users.Count} users");
    }
    
}