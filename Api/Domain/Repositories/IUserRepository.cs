namespace DefaultNamespace;

public interface IUserRepository : IRepository<User>
{
    User GetByEmail(string email);
}