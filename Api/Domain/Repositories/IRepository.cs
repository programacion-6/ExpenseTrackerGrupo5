using Api.Domain.Entities;

namespace DefaultNamespace;

public interface IRepository<T> where T : EntityBase
{
    bool Save (T item);
    bool Delete (T item);
    bool Update (T item);
    T GetById (Guid itemId);
    List<T> GetAll();
}