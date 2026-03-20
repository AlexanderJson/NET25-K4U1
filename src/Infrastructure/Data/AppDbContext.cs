using Microsoft.EntityFrameworkCore;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}