
using MyWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MyWebApi.Infrastructure.Repositories.Users;


public class SecretRepository : ISecretRepository
{
    private readonly AppDbContext _context;
    public SecretRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Secret secret)
    {
        _context.Secrets.Add(secret);
        _context.SaveChanges();
    }

    public Secret? GetByToken(byte[] HashedAccessToken)
    {
        return _context.Secrets.FirstOrDefault(
            x => x.HashedAccessToken.SequenceEqual(HashedAccessToken));

    }

    public void Update(Secret secret)
    {
        _context.Secrets.Update(secret);
        _context.SaveChanges();
    }

}





