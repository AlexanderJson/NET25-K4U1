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


    public virtual async Task<List<TGetDto>> GetAll()
    {
        var entities = await _repo.GetAll();
        return [.. entities.Select(ReturnDto)];
    }


    public virtual async Task<TGetDto?> GetById(Guid id)
    {
        _validateId(id);

        var entity = await _repo.GetById(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        return ReturnDto(entity);
    }

    public virtual async Task Delete(Guid id)
    {
        _validateId(id);

        var entity = await _repo.GetById(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        await _repo.Delete(entity);
    }



    public virtual async Task Add(TPostDto dto)
    {
        var entity = GetEntity(dto);
        await _repo.Add(entity);
    }



    public virtual async Task<TGetDto> Update(Guid id, TPostDto dto)
    {
        _validateId(id);

        var entity = await _repo.GetById(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        ApplyUpdate(entity, dto);

        await _repo.Update(entity);

        return ReturnDto(entity);
    }



    // Global helper I can use
    private static void _validateId(Guid id)
    {
        if(id == Guid.Empty)
            throw new InvalidIdException($"Invalid ID: {id}");
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

    public virtual async Task<PagedResult<TGetDto>> GetPaged(int page, int pageSize)
    {
        IQueryable<TEntity> query = ApplyOrdering(_repo.Query());

        var totalCount = query.Count();

        var data = query
            .ApplyPagination(page, pageSize)
            .ToList()
            .Select(ReturnDto);

        return new PagedResult<TGetDto>(
            data,
            totalCount,
            page,
            pageSize
        );
    }
}
