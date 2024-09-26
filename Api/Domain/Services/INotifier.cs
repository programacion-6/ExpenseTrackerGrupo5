namespace Api.Domain;

public interface INotifier<TContent>
{
    public void Notify(TContent content);
}