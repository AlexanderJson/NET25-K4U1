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

        Console.WriteLine("DEV: Created user:");
        Console.WriteLine($"       Id: {user.Id}");
        Console.WriteLine($"       Username: {user.Username}");
        Console.WriteLine($"       Email: {user.Email}");

        Console.WriteLine("dev: Created secret:");
        Console.WriteLine($"       SecretId: {secret.Id}");
        Console.WriteLine($"       RawAccessToken: {rawToken}");
        Console.WriteLine($"       HashedAccessToken(Base64): {Convert.ToBase64String(secret.HashedAccessToken)}");
        Console.WriteLine($"       ExpiresAt: {secret.ExpiresAt:O}");

        Console.WriteLine("dev: Runtime seed completed.");

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
}