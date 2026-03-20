namespace MyWebApi.App.Interfaces;


public interface IRepository<T>
{
    void Add(T data);
    T GetById(Guid id);

    void Update(T data);

    List<T> GetAll();

    void Delete(T data);
    
}