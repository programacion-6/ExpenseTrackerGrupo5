namespace Api.Domain;

public class BudgetException : Exception
{
    public BudgetException(string message) : base(message)
    { }
}