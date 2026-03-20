namespace MyWebApi.App.Interfaces;

public interface IService<T>
{
    T Add(T data);
    T getById(int id);

    List<T> GetAll();

}