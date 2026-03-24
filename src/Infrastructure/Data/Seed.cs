using MyWebApi.Domain.Entities;
using MyWebApi.Domain.Enums;

namespace MyWebApi.Infrastructure.Data;
/*
public static class Seed
{
    public static void SeedAll(AppDbContext db)
    {

        var random = new Random();
        List<User> users = SeedUsers();
        db.Users.AddRange(users);
        db.SaveChanges();

        List<Workspace> workspaces = SeedWorkspaces(users, random);
        db.Workspaces.AddRange(workspaces);
        db.SaveChanges();

        List<WorkSpaceMember> members = SeedWorkspaceMembers(workspaces, users, random);
        db.WorkspaceMembers.AddRange(members);
        db.SaveChanges();

        List<Document> documents = SeedDocuments(workspaces, members, random);
        db.Documents.AddRange(documents);
        db.SaveChanges();

        List<Invite> invites = SeedInvites(workspaces, random);
        db.Invites.AddRange(invites);
        db.SaveChanges();
    }

    private static List<User> SeedUsers()
    {
        var users = new List<User>();

        for (var i = 1; i <= 50; i++)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = $"user{i}",
                Email = $"user{i}@mail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            });
        }

        return users;
    }

    private static List<Workspace> SeedWorkspaces(List<User> users, Random random)
    {
        var workspaces = new List<Workspace>();

        for (var i = 1; i <= 20; i++)
        {
            User owner = users[random.Next(users.Count)];

            workspaces.Add(new Workspace
            {
                Id = Guid.NewGuid(),
                Name = $"Workspace {i}",
                Description = $"Mocked workspace {i} for testing documents.",
                OwnerId = owner.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60))
            });
        }

        return workspaces;
    }

    private static List<WorkSpaceMember> SeedWorkspaceMembers(List<Workspace> workspaces, List<User> users, Random random)
    {
        var members = new List<WorkSpaceMember>();

        foreach (Workspace workspace in workspaces)
        {
            var usedUserIds = new HashSet<Guid>();

             members.Add(new WorkSpaceMember
            {
                Id = Guid.NewGuid(),
                WorkspaceId = workspace.Id,
                UserId = workspace.OwnerId,
                Role = WorkspaceRole.Admin,
                JoinedAt = workspace.CreatedAt
            });

            usedUserIds.Add(workspace.OwnerId);

            int extraMembers = random.Next(2, 8);

            while (usedUserIds.Count < extraMembers + 1)
            {
                var user = users[random.Next(users.Count)];

                if (usedUserIds.Contains(user.Id))
                    continue;

                members.Add(new WorkSpaceMember
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspace.Id,
                    UserId = user.Id,
                    Role = random.Next(0, 2) == 0 ? WorkspaceRole.Viewer : WorkspaceRole.Editor,
                    JoinedAt = workspace.CreatedAt.AddDays(random.Next(0, 10))
                });

                usedUserIds.Add(user.Id);
            }
        }

        return members;
    }

    private static List<Document> SeedDocuments(List<Workspace> workspaces, List<WorkSpaceMember> members, Random random)
    {
        var documents = new List<Document>();

        foreach (Workspace workspace in workspaces)
        {
            var workspaceMembers = members
                .Where(m => m.WorkspaceId == workspace.Id)
                .ToList();

            var docCount = random.Next(2, 7);

            for (var i = 1; i <= docCount; i++)
            {
                WorkSpaceMember author = workspaceMembers[random.Next(workspaceMembers.Count)];

                documents.Add(new Document
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspace.Id,
                    CreatedById = author.UserId,
                    Title = $"Document {i} for {workspace.Name}",
                    Content = $"""
                    This is seeded content for {workspace.Name}.

                    Document number: {i}
                    Created for testing:
                    - test test
                    - test test
                    """,
                    CreatedAt = workspace.CreatedAt.AddDays(random.Next(0, 15)),
                    UpdatedAt = random.Next(0, 2) == 0 ? null : DateTime.UtcNow.AddDays(-random.Next(0, 10))
                });
            }
        }

        return documents;
    }

    private static List<Invite> SeedInvites(List<Workspace> workspaces, Random random)
    {
        var invites = new List<Invite>();

        int inviteCounter = 1;

        foreach (var workspace in workspaces)
        {
            int inviteCount = random.Next(0, 3);

            for (int i = 0; i < inviteCount; i++)
            {
                invites.Add(new Invite
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspace.Id,
                    Email = $"invite{inviteCounter}@mail.com",
                    Role = random.Next(0, 2) == 0 ? "Viewer" : "Editor",
                    Token = Guid.NewGuid().ToString("N"),
                    ExpiresAt = DateTime.UtcNow.AddDays(7 + random.Next(0, 14)),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 5))
                });

                inviteCounter++;
            }
        }

        return invites;
    }
}
*/