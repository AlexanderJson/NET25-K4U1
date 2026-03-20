using MyWebApi.App.DTO;
using MyWebApi.Domain.Entities;
using MyWebApi.App.Abstracts;
using MyWebApi.Api.Interfaces;


namespace MyWebApi.App.Services;
public class UserService : AService<CreateUserDto, UserDto, User>
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo) : base(userRepo)
    {
        _userRepo = userRepo;
    }

    protected override void ApplyUpdate(User entity, CreateUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Username))
            entity.Username = dto.Username;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);


        if (!string.IsNullOrWhiteSpace(dto.Name))
            entity.Name = dto.Name;
    }
    protected override UserDto MapToDto(User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Username = entity.Username
        };
    }
    protected override User MapToEntity(CreateUserDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
    }

    protected override void ValidateAdd(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ArgumentException("Username required!");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password required!");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name required!");

        if (_userRepo.IsUsernameTaken(dto.Username))
            throw new UsernameTakenException("User already exists.");
    }

    protected override void ValidateUpdate(User entity, CreateUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Username) &&
            dto.Username != entity.Username &&
            _userRepo.IsUsernameTaken(dto.Username))
        {
            throw new UsernameTakenException("Username already taken.");
        }
    }


}


