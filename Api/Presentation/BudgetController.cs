using System.Security.Claims;

using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly INotifier<EmailContent> _emailNotifier;

    public BudgetController(IMapper mapper, INotifier<EmailContent> emailNotifier)
    {
        _mapper = mapper;
        _emailNotifier = emailNotifier;
    }

    [HttpPost]
    public async Task<IActionResult> SetMonthlyBudget([FromBody] CreateBudgetRequest createBudgetRequest)
    {
        try
        {
            /* 
            This is just a test of how to use the email service,
            this code have to be removed
             */
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var subjectEmail = "Subject prove";
            var subjectBody = $"Body prove {createBudgetRequest.Currency}";
            var email = new EmailContent(userEmail, subjectEmail, subjectBody);
            await _emailNotifier.Notify(email);
            return Ok("Email sent");
        }
        catch (EmailNotificationException exception)
        {
            return StatusCode(500, exception.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentMonthBudget()
    {
        return Ok("budget");
    }
}
