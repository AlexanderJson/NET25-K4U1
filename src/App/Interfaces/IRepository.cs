namespace MyWebApi.App.Interfaces;


public interface IRepository<T>
{
    void Add(T data);
    T getById(Guid id);

    List<T> GetAll();


}