using Api.Domain;

namespace Api.Application;

public abstract class BaseTrackerChain<T> : ITrackingNotifier<T>
{
    private ITrackingNotifier<T>? _nextHandler;

    public virtual async Task NotifyTracking(T request)
    {
        if (_nextHandler != null)
        {
            await _nextHandler.NotifyTracking(request);
        }
    }

    public ITrackingNotifier<T> SetNext(ITrackingNotifier<T> handler)
    {
        _nextHandler = handler;
        return handler;
    }

}