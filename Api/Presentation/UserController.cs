using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest updateUserRequest)
    {
        return Ok("User profile updated successfully.");
    }

    [HttpPost("passwordreset")]
    public async Task<IActionResult> SendPasswordResetEmail([FromBody] string email)
    {
        return Ok("Password reset email sent.");
    }

    [HttpPost("passwordreset/verification")]
    public async Task<IActionResult> VerifyAndResetPassword([FromBody] PasswordResetVerificationRequest request)
    {
        return Ok("Password has been reset successfully.");
    }
}
