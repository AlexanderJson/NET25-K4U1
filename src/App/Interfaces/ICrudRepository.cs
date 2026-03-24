namespace MyWebApi.App.Interfaces;

/// <summary>
/// This interface is used with repositories in Infrastructure.
/// 
/// App is not supposed to "know" about infrastructure. Since there
/// is nothing in its responsibilities that requires this. 
/// Creating the interface here helps me with two things:
/// 1. Infrastructure/Repositories can change internal logic without breaking the app since it has to fulfill 
/// this contract. Since App/Service implements abstracted logic from Infrastructure/Repositories.
/// 2. Gives my App services a reference to the repository methods without knowing its inner logic,
/// so I can still use them, 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrudRepository<TEntity>
{

    IQueryable<TEntity> Query();
    void Add(TEntity data);
    TEntity GetById(Guid id);

    void Update(TEntity data);

    List<TEntity> GetAll();

    void Delete(TEntity data);
    
}