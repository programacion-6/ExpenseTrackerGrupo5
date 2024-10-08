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
    private readonly ITracker<Income, Budget> _tracker;

    public IncomesController(IIncomeService incomeService, IMapper mapper, ITracker<Income, Budget> tracker)
    {
        _incomeService = incomeService;
        _mapper = mapper;
        _tracker = tracker;
    }

    [HttpPost]
    public async Task<IActionResult> LogIncome([FromBody] CreateIncomeRequest createIncomeRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var income = _mapper.Map<Income>(createIncomeRequest);
        income.UserId = Guid.Parse(userId);

        var result = await _incomeService.CreateAsync(income);

        if (!result)
        {
            return StatusCode(500, "Error saving income.");
        }

        try
        {
            await _tracker.TrackNewUserEntry(income, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok($"Income logged successfully");

    }


    [HttpGet]
    public async Task<IActionResult> GetAllIncomes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest("User not found");
        }

        var incomes = await _incomeService.GetIncomesByUserIdAsync(Guid.Parse(userId));

        if (incomes == null || !incomes.Any())
        {
            return NotFound("No incomes found for the current user.");
        }

        var incomesResponse = _mapper.Map<IncomeResponse[]>(incomes);

        return Ok(incomesResponse);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest("User not found");
        }

        var income = await _incomeService.GetByIdAsync(id);

        if (income == null || income.UserId != Guid.Parse(userId))
        {
            return NotFound("Income not found or you do not have permission to access this income.");
        }

        var incomeResponse = _mapper.Map<IncomeResponse>(income);

        return Ok(incomeResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeRequest updateIncomeRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var oldIncome = await _incomeService.GetByIdAsync(id);

        if (oldIncome == null || oldIncome.UserId != Guid.Parse(userId))
        {
            return NotFound("Income not found or you do not have permission to update this income.");
        }

        var newIncome = _mapper.Map<Income>(updateIncomeRequest);
        newIncome.Id = oldIncome.Id;
        newIncome.UserId = oldIncome.UserId;

        var result = await _incomeService.UpdateAsync(newIncome);

        if (!result)
        {
            return StatusCode(500, "Error updating income.");
        }

        try
        {
            await _tracker.TrackUpdatedUserEntry(oldIncome, newIncome, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok("Income updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var income = await _incomeService.GetByIdAsync(id);
        if (income == null || income.UserId != Guid.Parse(userId))
        {
            return NotFound("Income not found or you do not have permission to delete this income.");
        }
        var result = await _incomeService.DeleteAsync(id);

        if (!result)
        {
            return StatusCode(500, "Error deleting income.");
        }

        try
        {
            await _tracker.TrackDeletedUserEntry(income, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok("Income deleted successfully.");
    }

}
