namespace Api.Domain;

public interface ITokenHandler
{
    public string GenerateToken(User user);
}