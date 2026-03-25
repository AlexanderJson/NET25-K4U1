using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;
using MyWebApi.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace MyWebApi.Infrastructure.Repositories.Users;

//TODO async?
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }


    public IQueryable<User> Query()
    {
        return _context.Users.AsNoTracking();;
    }






    public async Task Add(User user)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

     public async Task<List<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }



	public async Task<User?> GetById(Guid id)
	{
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("User not found");
    }

    public async Task Delete(User data)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == data.Id) ?? throw new Exception("User not found");
        _context.Remove(user);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> IsUsernameTaken(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }


    public async Task Update(User user)
    {
        _context.Users.Update(user); // Since I use GetById in Service it tracks the entity anyways, but kept this for safety
        await _context.SaveChangesAsync(); 
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}