namespace MyWebApi.App.Interfaces;

public interface IService<addDto, getDto>
{
    void Add(addDto data);
    getDto getById(Guid id);

    List<getDto> GetAll();

}