using MyWebApi.App.Services;

namespace MyWebApi.App.Interfaces;

// TPost = DTOs with more data for posting (ex PostUserDto would include otherwise hidden fields)
// TGet = the data sent back from db to client
public interface ICrudService<TPost, TGet>
{
    Task<PagedResult<TGet>> GetPaged(int page, int pageSize);
    Task<List<TGet>> GetAll();
    Task<TGet?> GetById(Guid id);
    Task Delete(Guid id);
    Task Add(TPost data);
    Task<TGet> Update(Guid id, TPost dto);
}
