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
    private readonly IBudgetService _budgetService;

    public BudgetController(IMapper mapper, IBudgetService budgetService)
    {
        _mapper = mapper;
        _budgetService = budgetService;

    }

    [HttpPost]
    public async Task<IActionResult> SetMonthlyBudget([FromBody] CreateBudgetRequest createBudgetRequest)
    {
        try
        {
            return Ok();
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

    [HttpGet("currentmonth")]
    public async Task<IActionResult> GetCurrentMonthBudget()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userGuidId = userId is not null ? Guid.Parse(userId) : default;
        try
        {
            var currentUserBudget = await _budgetService.GetCurrentUserBudget(userGuidId);
            return Ok(currentUserBudget);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
