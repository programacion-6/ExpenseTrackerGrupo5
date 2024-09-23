namespace Api.Domain;

public interface IRepository<T> where T : IEntityBase
{
    bool Save (T item);
    bool Delete (T item);
    bool Update (T item);
    T GetById (Guid itemId);
    List<T> GetAll();
}