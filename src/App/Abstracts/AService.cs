using MyWebApi.App.Interfaces;
using System.Linq;
namespace MyWebApi.App.Abstracts;

public abstract class AService<TPost, TGet, TEntity> : IService<TPost, TGet>
{
    protected readonly IRepository<TEntity> _repo;
    protected AService(IRepository<TEntity> repo)
    {
        _repo = repo;
    }


        public virtual List<TGet> GetAll()
        {
            return [.. _repo.GetAll().Select(MapToDto)];
        }

    public virtual TGet getById(Guid id)
    {
        if(id == Guid.Empty) throw new InvalidIdException($"ID is either empty or invalid type. Try again..! ID sent: {id}");
        TEntity e = _repo.GetById(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        return MapToDto(e);   
    }
    public virtual void Delete(Guid id)
    {
        _validateId(id);
        TEntity entity = _repo.GetById(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");;
        _repo.Delete(entity);
    }


    public virtual TGet Update(Guid id, TPost dto)
    {
        _validateId(id);
        _validateDto(dto);
        var entity = _repo.GetById(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        ValidateUpdate(entity,dto);
        ApplyUpdate(entity,dto);
        _repo.Update(entity);
        return MapToDto(entity);
    }

    public virtual void Add(TPost dto)
    {
        _validateDto(dto);

        ValidateAdd(dto);

        var entity = MapToEntity(dto);

        _repo.Add(entity);
    }
    protected abstract TEntity MapToEntity(TPost dto);
    protected abstract TGet MapToDto(TEntity entity);


    // Global helpers I can use
    private static void _validateId(Guid id)
    {
        if(id == Guid.Empty)
            throw new InvalidIdException($"Invalid ID: {id}");
    }
    private static void _validateDto(TPost dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");
    }
    protected virtual void ValidateAdd(TPost dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");
    }

    protected virtual void ValidateUpdate(TEntity entity, TPost dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");
    }

    protected abstract void ApplyUpdate(TEntity entity, TPost dto);

}

public class UsernameTakenException : Exception
{
    public UsernameTakenException(string msg) : base(msg) {}
}

public class NotFoundException : Exception
{
    public NotFoundException(string msg) : base(msg) {}
}

public class InvalidIdException : Exception
{
    public InvalidIdException(string msg) : base(msg) {}
}