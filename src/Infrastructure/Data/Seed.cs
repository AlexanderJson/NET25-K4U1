using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;

public static class Seed
{
    public static void SeedUsers(AppDbContext context)
    {

        var users = new List<User>();
        for (int i = 1; i <= 50; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = $"user{i}",
                Name = $"User {i}",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            });
        }
        context.Users.AddRange(users);
        context.SaveChanges();
    }
}