namespace Api.Domain;

public interface IRepository<T> where T : IEntityBase
{
    public Task<bool> Save(T item);
    public Task<bool> Delete(T item);
    public Task<bool> Update(T item);
    public Task<T?> GetById(Guid itemId);
    public Task<List<T>> GetAll();
}