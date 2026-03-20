namespace MyWebApi.App.Interfaces;


public interface IRepository<T>
{
    void Add(T data);
    T getById(int id);

    List<T> GetAll();

}