namespace Api.Domain;

public interface ITrackerChain<TTrackeable>
{
    public ITrackerChain<TTrackeable> SetNext(ITrackerChain<TTrackeable> handler);
    public Task Handle(TTrackeable trackeable);
}