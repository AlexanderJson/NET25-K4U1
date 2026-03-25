using MyWebApi.Api.Interfaces;
using MyWebApi.App.Interfaces;
namespace MyWebApi.App.Services;

public class AuthService(IUserRepository repo, IJwTService jwtService) : IAuth
{
    private readonly IUserRepository _repo = repo;
    private readonly IJwTService _jwtService = jwtService;



    public async Task<string> Login(string username, string password)
    {
        var user = (_repo.Query()
        .FirstOrDefault(u => u.Username == username) ?? throw new UnauthorizedAccessException()) ?? throw new UnauthorizedAccessException();
        
        if (!BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            throw new UnauthorizedAccessException();

        return _jwtService.GenerateToken(user.Id, user.Username);
    }
}
