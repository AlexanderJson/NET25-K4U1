using MyWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyWebApi.App.Querying;
namespace MyWebApi.Infrastructure.Repositories.Users;


public class SecretRepository : ISecretRepository
{
    private readonly AppDbContext _context;
    public SecretRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Secret secret)
    {
        await _context.Secrets.AddAsync(secret);
        await _context.SaveChangesAsync();
    }

    public async Task<Secret?> GetByToken(byte[] HashedAccessToken)
    {
        return await _context.Secrets.FirstOrDefaultAsync(
            x => x.HashedAccessToken.SequenceEqual(HashedAccessToken));

    }

    public async Task<List<UserSecretList>> GetUserSecrets(Guid userId)
    {
        return await _context.Secrets
            .OwnedBy(userId)
            .WhereActive()
            .ToUserList()
            .ToListAsync();
    }


    public async Task Update(Secret secret)
    {
        _context.Secrets.Update(secret);
        await _context.SaveChangesAsync();
    }

}