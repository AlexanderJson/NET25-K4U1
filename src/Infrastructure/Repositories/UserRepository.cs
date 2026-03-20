
using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;
namespace MyWebApi.Infrastructure.Repositories;


public class UserRepository : IRepository<User>
{
    private static List<User> _users = new();
    private static int _id = 1;

    public void Add(User user)
    {
        user.Id = _id++;
        _users.Add(user);
    }

    public List<User> GetAll()
    {
        return _users;
    }

	public User getById(int id)
	{
		throw new NotImplementedException();
	}

}