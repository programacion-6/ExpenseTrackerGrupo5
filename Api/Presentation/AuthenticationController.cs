using Api.Application;
using Api.Domain;

using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<User> _userValidator;

    public AuthenticationController(IMapper mapper, IAuthenticationService authenticationService, IValidator<User> userValidator)
    {
        _mapper = mapper;
        _authenticationService = authenticationService;
        _userValidator = userValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userToRegister = _mapper.Map<User>(createUserRequest);
            var validationResult = await _userValidator.ValidateAsync(userToRegister);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var token = await _authenticationService.Register(userToRegister);
            var expiresAt = DateTime.Now.AddDays(JwtConstants.ExpirationDays);
            var authResponse = new AuthResponse(token, expiresAt);

            return Ok(authResponse);
        }
        catch (AuthenticationException exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }
        catch (Exception exception)
        {
            return StatusCode(500, $"Internal server error: {exception.Message}");
        }

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var token = await _authenticationService.Login(loginRequest.Email, loginRequest.Password);
            var expiresAt = DateTime.Now.AddDays(JwtConstants.ExpirationDays);
            var authResponse = new AuthResponse(token, expiresAt);

            return Ok(authResponse);
        }
        catch (AuthenticationException exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
