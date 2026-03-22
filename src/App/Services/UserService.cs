using MyWebApi.App.DTO;
using MyWebApi.Domain.Entities;
using MyWebApi.App.Abstracts;
using MyWebApi.App.Exceptions;
using MyWebApi.Api.Interfaces;

namespace MyWebApi.App.Services;

public class UserService(IUserRepository userRepo)
    : AService<CreateUserDto, UserDto, User>(userRepo)
{
    private readonly IUserRepository _userRepo = userRepo;

    protected override void ApplyUpdate(User entity, CreateUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Username))
            entity.Username = dto.Username;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            entity.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            entity.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    }

    protected override UserDto ReturnDto(User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email
        };
    }

    protected override User GetEntity(CreateUserDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
    }

    protected override void ValidateArgs(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ArgumentException("Username required.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password required.");

        if (_userRepo.IsUsernameTaken(dto.Username))
            throw new UsernameTakenException("Username already exists.");

        if (_userRepo.IsEmailTaken(dto.Email))
            throw new ArgumentException("Email already exists.");
    }

    protected override void ValidateUpdate(User entity, CreateUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Username) &&
            dto.Username != entity.Username &&
            _userRepo.IsUsernameTaken(dto.Username))
        {
            throw new UsernameTakenException("Username already taken.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            dto.Email != entity.Email &&
            _userRepo.IsEmailTaken(dto.Email))
        {
            throw new ArgumentException("Email already taken.");
        }
    }

    protected override IQueryable<User> ApplyOrdering(IQueryable<User> query)
    {
        return query.OrderBy(u => u.Username);
    }
}