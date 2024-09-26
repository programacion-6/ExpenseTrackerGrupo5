namespace Api.Domain;

public class EmailNofiticationException : Exception
{
    public EmailNofiticationException(string message) : base(message)
    { }
}