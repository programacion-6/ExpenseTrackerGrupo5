using Api.Domain;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingHandler _hashingHandler;
    private readonly ITokenHandler _tokenHandler;

    public AuthenticationService(IUserRepository userRepository, IHashingHandler hashingHandler, ITokenHandler tokenHandler)
    {
        _userRepository = userRepository;
        _hashingHandler = hashingHandler;
        _tokenHandler = tokenHandler;
    }

    public async Task<string> Register(User user)
    {
        var userFound = await _userRepository.GetByEmail(user.Email);
        if (userFound is null)
        {
            user.Email = user.Email.ToLower();
            user.PasswordHash = _hashingHandler.Hash(user.PasswordHash);
            var wasRegistered = await _userRepository.Save(user);

            return !wasRegistered
                ? throw new Exception("An error occurred while registering the user")
                : _tokenHandler.GenerateToken(user);
        }

        throw new AuthenticationException("The email has already been registered");
    }

    public async Task<string> Login(string email, string password)
    {
        var userFound = await _userRepository.GetByEmail(email);
        if (
            userFound is not null && 
            _hashingHandler.MatchHashing(password, userFound.PasswordHash))
        {
            var token = _tokenHandler.GenerateToken(userFound);

            return token;
        }

        throw new AuthenticationException("Invalid email or password");
    }
}