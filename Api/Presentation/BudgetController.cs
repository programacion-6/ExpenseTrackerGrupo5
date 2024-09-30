using System.Security.Claims;

using Api.Domain;

using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBudgetService _budgetService;
    private readonly IValidator<Budget> _budgetValidator;

    public BudgetController(IMapper mapper, IBudgetService budgetService, IValidator<Budget> budgetValidator)
    {
        _mapper = mapper;
        _budgetService = budgetService;
        _budgetValidator = budgetValidator;
    }

    [HttpPost]
    public async Task<IActionResult> SetMonthlyBudget([FromBody] CreateBudgetRequest createBudgetRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }


        var newBudget = _mapper.Map<Budget>(createBudgetRequest);
        var validationResult = await _budgetValidator.ValidateAsync(newBudget);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        try
        {
            newBudget.UserId = Guid.Parse(userId);
            await _budgetService.AddMonthlyUserBudget(newBudget);
            return Ok("Established monthly budget");
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
            var budgetResponse = _mapper.Map<BudgetResponse>(currentUserBudget);
            return Ok(budgetResponse);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
