namespace Api.Domain;

public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message)
    { }
}