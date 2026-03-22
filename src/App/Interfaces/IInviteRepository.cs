using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Api.Interfaces;

public interface IInviteRepository : ICrudRepository<Invite>
{
    bool TokenExists(string token);
}