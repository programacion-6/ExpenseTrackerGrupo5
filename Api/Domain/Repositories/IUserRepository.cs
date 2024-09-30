namespace Api.Domain;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmail(string email);
}