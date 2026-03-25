
namespace MyWebApi.App.Interfaces;


public interface IUserService<TPost, TGet> : ICrudService<TPost, TGet>
{
    Task<IEnumerable<TGet>> SearchByUsername(string username);

    Task<string> GeneratePassword();

    Task SetPassword(Guid id, string password);
}
