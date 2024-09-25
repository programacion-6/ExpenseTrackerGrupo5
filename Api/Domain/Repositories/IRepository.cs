namespace Api.Domain;

public interface IRepository<T> where T : IEntityBase
{
    Task<bool> Save (T item);
    Task<bool> Delete (T item);
    Task<bool> Update (T item);
    T GetById (Guid itemId);
    List<T> GetAll();
}