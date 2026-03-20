using MyWebApi.App.DTO;
using BCrypt.Net;
using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;

namespace MyWebApi.App.Services;
public class UserService : IService<CreateUserDto, UserDto>
{
    private readonly IRepository<User> _repo;
    public UserService(IRepository<User> repo)
    {
        _repo = repo;
    }

    public void Add(CreateUserDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _repo.Add(user);
         //TODO nice response med wrapper
    }

    public List<UserDto> GetAll()
    {
        return _repo.GetAll()
            .Select(u => new UserDto
            {
                Name = u.Name,
                Username = u.Username,
                Password = u.Password
            })
            .ToList();
    }

    public UserDto getById(Guid id)
    {
        var user = _repo.getById(id);
        return new UserDto
        {
            Name = user.Name,
            Username = user.Username,
            
        };  
        
    }
}


