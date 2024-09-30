namespace Api.Domain;

public interface INotifier<TContent>
{
    public Task Notify(TContent content);
}
