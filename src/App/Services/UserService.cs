using MyWebApi.App.DTO;
using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;

namespace MyWebApi.App.Services;
public class UserService : IService<UserDto>
{
    private readonly IRepository<User> _repo;
    public UserService(IRepository<User> repo)
    {
        _repo = repo;
    }


    UserDto IService<UserDto>.Add(UserDto dto)
    {
        var user = new User
        {
            Name = dto.Name
        };
        _repo.Add(user);
        return dto; //TODO!
    }

    List<UserDto> IService<UserDto>.GetAll()
    {
        return _repo.GetAll()
            .Select(u => new UserDto
            {
                Name = u.Name
            })
            .ToList();
    }

    UserDto IService<UserDto>.getById(int id)
    {
        var user = _repo.getById(id);
        return new UserDto
        {
            Name = user.Name
        };
        
    }
}