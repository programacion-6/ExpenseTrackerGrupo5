namespace Api.Domain;

public class EmailNotificationException : Exception
{
    public EmailNotificationException(string message) : base(message)
    { }
}