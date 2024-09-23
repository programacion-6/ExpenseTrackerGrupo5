using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMapper _mapper;

    public AuthenticationController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest)
    {
        return Ok("No implemented");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        return Ok("No implemented");
    }
}
