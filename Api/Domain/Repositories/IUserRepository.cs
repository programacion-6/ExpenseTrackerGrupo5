namespace Api.Domain;

public interface IUserRepository : IRepository<User>
{
    User GetByEmail(string email);
}