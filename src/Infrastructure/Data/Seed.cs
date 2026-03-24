using System.Security.Cryptography;
using System.Text;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Infrastructure.Data;

public static class Seed
{
    public static void SeedAll(AppDbContext db)
    {
        Console.WriteLine("[SEED] SeedAll started.");

        if (db.Users.Any())
        {
            Console.WriteLine("[SEED] Users already exist. Skipping seed.");
            return;
        }

        var demoUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "DemoUser1",
            Email = "demo1@test.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456")
        };

        Console.WriteLine($"[SEED] Creating demo user:");
        Console.WriteLine($"       Username: {demoUser.Username}");
        Console.WriteLine($"       Email: {demoUser.Email}");
        Console.WriteLine($"       Id: {demoUser.Id}");

        db.Users.Add(demoUser);

        var rawTokens = new[]
        {
            "demo-token-1",
            "demo-token-2",
            "demo-token-3",
            "demo-token-4",
            "demo-token-5"
        };

        var now = DateTime.UtcNow;

        var secrets = new List<Secret>
        {
            new Secret
            {
                Id = Guid.NewGuid(),
                OwnerId = demoUser.Id,
                EncryptedContent = "demo-encrypted-1",
                HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawTokens[0])),
                ExpiresAt = now.AddHours(1),
                MaxViews = 1,
                CurrentViews = 0,
                RequiresPassword = false,
                CreatedAt = now
            },
            new Secret
            {
                Id = Guid.NewGuid(),
                OwnerId = demoUser.Id,
                EncryptedContent = "demo-encrypted-2",
                HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawTokens[1])),
                ExpiresAt = now.AddHours(2),
                MaxViews = 3,
                CurrentViews = 0,
                RequiresPassword = true,
                CreatedAt = now
            },
            new Secret
            {
                Id = Guid.NewGuid(),
                OwnerId = demoUser.Id,
                EncryptedContent = "demo-encrypted-3",
                HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawTokens[2])),
                ExpiresAt = now.AddDays(1),
                MaxViews = null,
                CurrentViews = 0,
                RequiresPassword = false,
                CreatedAt = now
            },
            new Secret
            {
                Id = Guid.NewGuid(),
                OwnerId = demoUser.Id,
                EncryptedContent = "demo-encrypted-4",
                HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawTokens[3])),
                ExpiresAt = now.AddMinutes(30),
                MaxViews = 2,
                CurrentViews = 1,
                RequiresPassword = false,
                CreatedAt = now
            },
            new Secret
            {
                Id = Guid.NewGuid(),
                OwnerId = demoUser.Id,
                EncryptedContent = "demo-encrypted-5",
                HashedAccessToken = SHA256.HashData(Encoding.UTF8.GetBytes(rawTokens[4])),
                ExpiresAt = now.AddDays(7),
                MaxViews = 10,
                CurrentViews = 0,
                RequiresPassword = true,
                CreatedAt = now
            }
        };

        for (var i = 0; i < secrets.Count; i++)
        {
            var secret = secrets[i];

            Console.WriteLine($"[SEED] Creating secret #{i + 1}");
            Console.WriteLine($"       SecretId: {secret.Id}");
            Console.WriteLine($"       RawAccessToken: {rawTokens[i]}");
            Console.WriteLine($"       HashedAccessToken(Base64): {Convert.ToBase64String(secret.HashedAccessToken)}");
            Console.WriteLine($"       ExpiresAt: {secret.ExpiresAt:O}");
            Console.WriteLine($"       MaxViews: {(secret.MaxViews?.ToString() ?? "null")}");
            Console.WriteLine($"       CurrentViews: {secret.CurrentViews}");
            Console.WriteLine($"       RequiresPassword: {secret.RequiresPassword}");
        }

        db.Secrets.AddRange(secrets);
        db.SaveChanges();

        Console.WriteLine("[SEED] SaveChanges completed.");
        Console.WriteLine($"[SEED] Inserted 1 user and {secrets.Count} secrets.");
        Console.WriteLine("[SEED] SeedAll finished.");
    }
}