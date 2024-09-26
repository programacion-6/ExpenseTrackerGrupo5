namespace Api.Domain;

public interface IHashingHandler
{
    public string Hash(string text);
    public bool MatchHashing(string originalText, string hashingText);
}