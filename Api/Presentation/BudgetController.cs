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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userEmail is null)
        {
            return BadRequest("User not found");
        }

        var newBudget = _mapper.Map<Budget>(createBudgetRequest);

        try
        {
            await _budgetService.AddMonthlyUserBudget(newBudget);
            return Ok();
        }
        catch (BudgetException exception)
        {
            return StatusCode(400, exception.Message);
        }
        catch (UserBudgetException exception)
        {
            return StatusCode(400, exception.Message);
        }
        catch (Exception exception)
        {
            return StatusCode(500, $"Internal server error: {exception.Message}");
        }
    }

    [HttpGet("currentmonth")]
    public async Task<IActionResult> GetCurrentMonthBudget()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest("user not found");
        }

        try
        {
            var userGuidId = Guid.Parse(userId);
            var currentUserBudget = await _budgetService.GetCurrentUserBudget(userGuidId);
            return Ok(currentUserBudget);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
