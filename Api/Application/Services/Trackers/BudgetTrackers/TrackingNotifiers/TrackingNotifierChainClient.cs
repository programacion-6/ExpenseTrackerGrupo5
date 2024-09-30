using Api.Domain;

namespace Api.Application;

public class TrackingNotifierChainClient
{
    private readonly string _userEmail;
    private readonly INotifier<EmailContent> _notifier;

    public TrackingNotifierChainClient(INotifier<EmailContent> notifier, string userEmail)
    {
        _notifier = notifier;
        _userEmail = userEmail;
    }

    public void Handle(Budget budget)
    {
        var increaseTracker = new BudgetIncreaseTracker(_notifier, _userEmail);
        var decreaseTracker = new BudgetDecreaseTracker(_notifier, _userEmail);
        decreaseTracker.SetNext(increaseTracker);

        try
        {
            Task.Run(async () => await decreaseTracker.NotifyTracking(budget));
        }
        catch (Exception exception)
        {
            // Logging the tracking errors
            Console.WriteLine(exception.Message);
        }
    }

}