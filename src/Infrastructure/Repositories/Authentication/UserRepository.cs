
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Interfaces;
namespace MyWebApi.Infrastructure.Repositories;


public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

	public User GetById(Guid id)
	{
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            throw new Exception("User not found");

        return user;
    }

    public void Delete(User data)
    {   
        var user = _context.Users.FirstOrDefault(u => u.Id == data.Id);

        if (user == null) throw new Exception("User not found");

        _context.Remove(user);
        _context.SaveChanges();
    }


    // TODO : can make async later - convert "Add" and "Update" to return Task 
    public bool IsUsernameTaken(string username)
    {
        return  _context.Users.Any(u => u.Username == username);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
