
using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;
using MyWebApi.Infrastructure.Data;

namespace MyWebApi.Infrastructure.Repositories;


public class UserRepository : IRepository<User>
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    private static List<User> _users = new();

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

	public User getById(Guid id)
	{
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            throw new Exception("User not found");

        return user;
        }

}