
using Api.Domain;

namespace Api.Application;

public class BudgetTracker : ITracker<Budget>
{
    private readonly string _userEmail;
    private readonly INotifier<EmailContent> _notifier;

    public BudgetTracker(INotifier<EmailContent> notifier, string userEmail)
    {
        _notifier = notifier;
        _userEmail = userEmail;
    }

    public async Task Track(Budget budget)
    {
        var increaseTracker = new BudgetIncreaseTracker(_notifier, _userEmail);
        var decreaseTracker = new BudgetDecreaseTracker(_notifier, _userEmail);
        decreaseTracker.SetNext(increaseTracker);

        try
        {
            await increaseTracker.Handle(budget);
        }
        catch (Exception exception)
        {
            // Logging the tracking errors
            Console.WriteLine(exception.Message);
        }
    }

}