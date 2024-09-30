using Api.Domain;

namespace Api.Application;

public class PasswordHashingHandler : IHashingHandler
{
    public string Hash(string text)
    {
        var hashedText = BCrypt.Net.BCrypt.HashPassword(text);

        return hashedText;
    }

    public bool MatchHashing(string originalText, string hashingText)
    {
        var isMatch = BCrypt.Net.BCrypt.Verify(originalText, hashingText);

        return isMatch;
    }

}