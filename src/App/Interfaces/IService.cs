using MyWebApi.App.Services;

namespace MyWebApi.App.Interfaces;

public interface IService<TPost, TGet>
{
    PagedResult<TGet> GetPaged(int page, int pageSize);
    List<TGet> GetAll();
    TGet GetById(Guid id);
    void Delete(Guid id);
    void Add(TPost data);
    TGet Update(Guid id, TPost dto);
}