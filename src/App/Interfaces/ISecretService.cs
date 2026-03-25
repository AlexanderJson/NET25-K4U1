using MyWebApi.App.DTO;
namespace MyWebApi.Api.Interfaces;
public interface ISecretService
{
    Task<CreatedSecretDto> CreateSecret(CreateSecretDto dto, Guid? id);
    Task<SecretDto> GetByToken(string accessToken);
    Task<List<UserSecretList>> GetByUserId(Guid userId);


}