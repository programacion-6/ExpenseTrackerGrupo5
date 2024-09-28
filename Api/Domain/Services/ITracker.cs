namespace Api.Domain;

public interface ITracker<TTrackeable>
{
    public Task Track(TTrackeable trackeable);
}