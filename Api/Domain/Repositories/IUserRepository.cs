namespace DefaultNamespace;

public interface IUserRepository
{
    User GetByEmail(string email);
}