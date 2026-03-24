
namespace MyWebApi.App.Interfaces;


public interface IUserService<TPost, TGet> : ICrudService<TPost, TGet>
{
    IEnumerable<TGet> SearchByUsername(string username);

    Task<string> GeneratePassword();

    void SetPassword(Guid id, string password);
}
