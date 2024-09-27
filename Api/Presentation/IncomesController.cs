using System.Security.Claims;

using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Api.Domain.Services;

[ApiController]
[Route("api/incomes")]
[Authorize]
public class IncomesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IIncomeService _incomeService;

    public IncomesController(IIncomeService incomeService, IMapper mapper)
    {
        _incomeService = incomeService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> LogIncome([FromBody] CreateIncomeRequest createIncomeRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var income = _mapper.Map<Income>(createIncomeRequest);
        income.UserId = Guid.Parse(userId);

        var result = await _incomeService.CreateAsync(income);

        if (result)
        {
            return Ok($"Income logged successfully");
        }
        
        return StatusCode(500, "Error saving income.");
    }


    [HttpGet]
    public async Task<IActionResult> GetAllIncomes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var incomes = await _incomeService.GetIncomesByUserIdAsync(Guid.Parse(userId));

        if (incomes == null || !incomes.Any())
        {
            return NotFound("No incomes found for the current user.");
        }

        return Ok(incomes);
    }


    [HttpGet("{id}")]
public async Task<IActionResult> GetIncomeById(Guid id)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var income = await _incomeService.GetByIdAsync(id);

    if (income == null || income.UserId != Guid.Parse(userId))
    {
        return NotFound("Income not found or you do not have permission to access this income.");
    }

    return Ok(income);
}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeRequest updateIncomeRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var income = await _incomeService.GetByIdAsync(id);

        if (income == null || income.UserId != Guid.Parse(userId))
        {
            return NotFound("Income not found or you do not have permission to update this income.");
        }
        
        income.Currency = updateIncomeRequest.Currency;
        income.Source = updateIncomeRequest.Source;
        income.Amount = updateIncomeRequest.Amount;
        income.Date = updateIncomeRequest.Date;

        var result = await _incomeService.UpdateAsync(income);

        if (result)
        {
            return Ok("Income updated successfully.");
        }

        return StatusCode(500, "Error updating income.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var income = await _incomeService.GetByIdAsync(id);
        if (income == null || income.UserId != userId)
        {
            return NotFound("Income not found or you do not have permission to delete this income.");
        }
        var result = await _incomeService.DeleteAsync(id); 
        if (result)
        {
            return Ok("Income deleted successfully.");
        }
        return StatusCode(500, "Error deleting income.");
    }

}
