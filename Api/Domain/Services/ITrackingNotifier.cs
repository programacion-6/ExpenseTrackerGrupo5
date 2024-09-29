namespace Api.Domain;

public interface ITrackingNotifier<T>
{
    public ITrackingNotifier<T> SetNext(ITrackingNotifier<T> handler);
    public Task NotifyTracking(T trackeable);
}