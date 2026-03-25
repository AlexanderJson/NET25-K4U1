using MyWebApi.App.DTO;
using MyWebApi.Domain.Entities;
using MyWebApi.App.Abstracts;
using MyWebApi.App.Exceptions;
using MyWebApi.Api.Interfaces;
using System.Text.Json;
using MyWebApi.App.Querying;
using Microsoft.Extensions.Caching.Distributed;
using MyWebApi.App.External.Quantum;
using MyWebApi.App.Interfaces;
namespace MyWebApi.App.Services;

public class UserService(
    IUserRepository userRepo,
    IDistributedCache cache,
    IQuantumService quantumS)
    : AService<CreateUserDto, UserDto, User>(userRepo),
      IUserService<CreateUserDto, UserDto>
{
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IQuantumService _quantumService = quantumS;
    private readonly IDistributedCache _cache = cache;


    protected override void ApplyUpdate(User entity, CreateUserDto dto)
    {
        entity.Username = dto.Username;
        entity.Email = dto.Email;
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

    /// <summary>
    /// We always hash password when returning entity.
    /// This method is used for POST/PUT/PATCH
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
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

        /// <summary>
        /// Just a temporary password generator. Uses
        /// an API that generates quantum numbers based 
        /// on quantum fluctuations. 
        /// 
        /// I wouldn't really use this in a real API
        /// since theres libraries that can still achieve
        /// high entropy randomness without the effort and risks.
        /// 
        /// But it was a fun implementaton to test.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GeneratePassword()
        {
            var random = await _quantumService.GetRandomSeedAsync();

            if (random == null || random.Data.Length == 0)
                throw new Exception("Quantum API failed");
            // some basic conversion incase the data was leaked 
            var bytes = random.Data
                .Take(4) 
                .SelectMany(BitConverter.GetBytes)
                .ToArray();

            return Convert.ToBase64String(bytes);
        }

    private async Task ValidateCreateAsync(CreateUserDto dto)
    {
        ExceptionHelper.ThrowIfUsernameEmptyOrWhiteSpace(dto.Username);
        ExceptionHelper.ThrowIfUsernameEmptyOrWhiteSpace(dto.Email);
        ExceptionHelper.ThrowIfUsernameEmptyOrWhiteSpace(dto.Password);

        if (await _userRepo.IsUsernameTaken(dto.Username))
            throw new UsernameTakenException("Username already exists.");

        if (await _userRepo.IsEmailTaken(dto.Email))
            throw new ArgumentException("Email already exists.");
    }

    private async Task ValidateUpdateAsync(User entity, CreateUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Username) &&
            dto.Username != entity.Username &&
            await _userRepo.IsUsernameTaken(dto.Username))
        {
            throw new UsernameTakenException("Username already taken.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            dto.Email != entity.Email &&
            await _userRepo.IsEmailTaken(dto.Email))
        {
            throw new ArgumentException("Email already taken.");
        }
    }
    public override async Task Add(CreateUserDto dto)
    {
        await ValidateCreateAsync(dto);

        var entity = GetEntity(dto);

        await _userRepo.Add(entity);
    }

    public override async Task<UserDto> Update(Guid id, CreateUserDto dto)
    {
        if (id == Guid.Empty)
            throw new InvalidIdException($"Invalid ID: {id}");

        var entity = await _userRepo.GetById(id)
            ?? throw new NotFoundException("User not found");

        await ValidateUpdateAsync(entity, dto);

        ApplyUpdate(entity, dto);

        await _userRepo.Update(entity);

        return ReturnDto(entity);
    }

    public async Task SetPassword(Guid userId, string rawPassword)
    {
        var user = await _userRepo.GetById(userId) ?? throw new NotFoundException("User not found");
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

        await _userRepo.Update(user);
    }

    protected override IQueryable<User> ApplyOrdering(IQueryable<User> query)
    {
        return query.OrderBy(u => u.Username);
    }




    public override async Task<PagedResult<UserDto>> GetPaged(int page, int pageSize)
    {
        var cacheKey = $"users:p{page}:s{pageSize}";

        var cachedJson = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrWhiteSpace(cachedJson))
        {
            var cached = JsonSerializer.Deserialize<PagedResult<UserDto>>(cachedJson);
            if (cached is not null)
                return cached;
        }

        var result = await base.GetPaged(page, pageSize);

        var json = JsonSerializer.Serialize(result);

        await _cache.SetStringAsync(
            cacheKey,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        return result;
    }



    public async Task<IEnumerable<UserDto>> SearchByUsername(string username)
    {
        ExceptionHelper.ThrowIfUsernameEmptyOrWhiteSpace(username);

        var users = _repo.Query()
            .FilterByUsername(username)
            .ToList();

        return users.Select(ReturnDto);
    }

    
}