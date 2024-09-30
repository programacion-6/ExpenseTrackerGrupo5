namespace Api.Domain;

public class RecurrentExpense : Expense
{
    public DateTime LastCalculation { get; set; }
}
