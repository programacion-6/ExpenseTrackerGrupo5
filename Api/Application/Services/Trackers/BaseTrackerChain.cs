using Api.Domain;

namespace Api.Application;

public abstract class BaseTrackerChain<TTrackeable> : ITrackerChain<TTrackeable>
{
    private ITrackerChain<TTrackeable>? _nextHandler;

    public virtual async Task Handle(TTrackeable request)
    {
        if (_nextHandler != null)
        {
            await _nextHandler.Handle(request);
        }
    }

    public ITrackerChain<TTrackeable> SetNext(ITrackerChain<TTrackeable> handler)
    {
        _nextHandler = handler;
        return handler;
    }

}