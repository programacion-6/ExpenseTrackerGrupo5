namespace Api.Domain;

public interface IAuthenticationService
{
    public Task<string> Register(User user);
    public Task<string> Login(string email, string password);
}