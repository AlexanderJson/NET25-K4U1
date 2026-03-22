using Microsoft.EntityFrameworkCore;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<WorkSpaceMember> WorkspaceMembers => Set<WorkSpaceMember>();
    public DbSet<Invite> Invites => Set<Invite>();

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

            entity.HasMany(u => u.OwnedWorkspaces)
                .WithOne(w => w.Owner)
                .HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.WorkspaceMemberships)
                .WithOne(wm => wm.User)
                .HasForeignKey(wm => wm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.CreatedDocuments)
                .WithOne(d => d.CreatedBy)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Workspace>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Description)
                .HasMaxLength(500);

            entity.HasMany(w => w.Documents)
                .WithOne(d => d.Workspace)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(w => w.Members)
                .WithOne(wm => wm.Workspace)
                .HasForeignKey(wm => wm.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(w => w.Invites)
                .WithOne(i => i.Workspace)
                .HasForeignKey(i => i.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(d => d.Content)
                .IsRequired()
                .HasMaxLength(5000);
        });

        modelBuilder.Entity<WorkSpaceMember>(entity =>
        {
            entity.HasKey(wm => wm.Id);

            entity.Property(wm => wm.Role)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(wm => new { wm.WorkspaceId, wm.UserId })
                .IsUnique();
        });

        modelBuilder.Entity<Invite>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(i => i.Role)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(i => i.Token)
                .IsRequired();

            entity.HasIndex(i => i.Token)
                .IsUnique();
        });
    }
}