using MyWebApi.App.DTO;
namespace MyWebApi.Api.Interfaces;
public interface ISecretService
{
    CreatedSecretDto CreateSecret(CreateSecretDto dto);
    SecretDto GetByToken(string accessToken);
}