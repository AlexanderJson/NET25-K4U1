using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;
namespace MyWebApi.Api.Interfaces;

/*
    Since I use a CRUD interface for repositories, I created this as an extension. 
*/
public interface IUserRepository : ICrudRepository<User>
{
    bool IsUsernameTaken(string username);
    bool IsEmailTaken(string email);

}