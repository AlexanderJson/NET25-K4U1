using Microsoft.EntityFrameworkCore;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Secret> Secrets => Set<Secret>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.HashedPassword)
                .IsRequired();

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.HasMany(u => u.Secrets)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Secret>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.HashedAccessToken)
                .IsRequired();

            entity.Property(s => s.EncryptedContent)
                .IsRequired();

            entity.Property(s => s.CurrentViews)
                .IsRequired();

            entity.Property(s => s.ExpiresAt)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .IsRequired();

            entity.Property(s => s.RequiresPassword)
                .IsRequired();

            entity.HasIndex(s => s.HashedAccessToken)
                .IsUnique();
        });
    }
}