using MyWebApi.App.Services;

namespace MyWebApi.App.Interfaces;

// TPost = DTOs with more data for posting (ex PostUserDto would include otherwise hidden fields)
// TGet = the data sent back from db to client
public interface ICrudService<TPost, TGet>
{
    PagedResult<TGet> GetPaged(int page, int pageSize); //TODO
    List<TGet> GetAll();
    TGet GetById(Guid id);
    void Delete(Guid id);
    void Add(TPost data);
    TGet Update(Guid id, TPost dto);
}