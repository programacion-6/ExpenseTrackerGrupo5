namespace Api.Domain;

public class UserBudgetException : Exception
{
    public UserBudgetException(string message) : base(message)
    { }
}