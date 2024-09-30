namespace Api.Domain;

public interface ITracker<TEntry, TTrackable>
{
    public Task TrackNewUserEntry(TEntry entry, string userEmail);
    public Task TrackUpdatedUserEntry(TEntry oldEntry, TEntry newEntry, string userEmail);
    public Task TrackDeletedUserEntry(TEntry entry, string userEmail);
    public Task NotifyTrackingToUser(TTrackable trackable, string userEmail);
}