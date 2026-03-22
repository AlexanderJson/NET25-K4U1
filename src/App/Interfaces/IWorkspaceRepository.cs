using MyWebApi.App.Interfaces;
using MyWebApi.Domain.Entities;

namespace MyWebApi.Api.Interfaces;

public interface IWorkspaceRepository : ICrudRepository<Workspace>
{
}