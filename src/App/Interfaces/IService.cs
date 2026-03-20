namespace MyWebApi.App.Interfaces;

public interface IService<TPost, TGet>
{
    List<TGet> GetAll();
    TGet getById(Guid id);
    void Delete(Guid id);
    void Add(TPost data);
    TGet Update(Guid id, TPost dto);
}