using MyWebApi.App.Interfaces;
using MyWebApi.App.Services;
using MyWebApi.App.Exceptions;
using MyWebApi.App.Querying;
namespace MyWebApi.App.Abstracts;

public abstract class AService<TPostDto, TGetDto, TEntity> : ICrudService<TPostDto, TGetDto>
{
    protected readonly ICrudRepository<TEntity> _repo;
    protected AService(ICrudRepository<TEntity> repo)
    {
        _repo = repo;
    }


    public virtual List<TGetDto> GetAll()
    {
        return [.. _repo.GetAll().Select(ReturnDto)];
    }

    public virtual TGetDto GetById(Guid id)
    {
        if(id == Guid.Empty) throw new InvalidIdException($"ID is either empty or invalid type. Try again..! ID sent: {id}");
        TEntity e = _repo.GetById(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        return ReturnDto(e);   
    }
    public virtual void Delete(Guid id)
    {
        _validateId(id);
        TEntity entity = _repo.GetById(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");;
        _repo.Delete(entity);
    }



    public virtual void Add(TPostDto dto)
    {
        ValidateArgs(dto);

        var entity = GetEntity(dto);

        _repo.Add(entity);
    }



    public virtual TGetDto Update(Guid id, TPostDto dto)
    {
        _validateId(id);
        ValidateArgs(dto);
        var entity = _repo.GetById(id);
        ApplyUpdate(entity,dto);
        _repo.Update(entity);
        return ReturnDto(entity);
    }



    // Global helpers I can use
    private static void _validateId(Guid id)
    {
        if(id == Guid.Empty)
            throw new InvalidIdException($"Invalid ID: {id}");
    }

    protected virtual void ValidateArgs(TPostDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");
    }

    protected virtual void ValidateUpdate(TEntity entity, TPostDto dto)
    {
        if (dto == null)
            throw new ArgumentException("DTO cannot be null");
    }


    protected virtual IQueryable<TEntity> ApplyOrdering
    (
        IQueryable<TEntity> query
    )
    {
        return query;
    }

    
    protected abstract TEntity GetEntity(TPostDto dto);
    protected abstract TGetDto ReturnDto(TEntity entity);

    protected abstract void ApplyUpdate(TEntity entity, TPostDto dto);

    public virtual  PagedResult<TGetDto> GetPaged(int page, int pageSize)
    {
        IQueryable<TEntity> query = ApplyOrdering(_repo.Query());
        var totalCount = query.Count();
        IEnumerable<TGetDto> data = query
            .ApplyPagination(page, pageSize)
            .ToList()
            .Select(ReturnDto);
        return new PagedResult<TGetDto>
        (
            data,
            totalCount,
            page,
            pageSize
        );
        
    }
}
