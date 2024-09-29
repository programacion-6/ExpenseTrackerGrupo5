using System.Security.Claims;

using Api.Domain;
using Api.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IExpenseService _expenseService;

    public ExpensesController(IMapper mapper, IExpenseService expenseService)
    {
        _mapper = mapper;
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (createExpenseRequest == null)
                return BadRequest("Invalid expense request.");

            var expense = _mapper.Map<Expense>(createExpenseRequest);

            expense.Id = Guid.NewGuid();
            expense.CreatedAt = DateTime.UtcNow;

            var success = await _expenseService.CreateAsync(expense);

            if (success)
                return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, _mapper.Map<ExpenseResponse>(expense));

            return StatusCode(500, "An error occurred while creating the expense.");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        try
        {
            var expenses = await _expenseService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        try
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
                return NotFound();

            return Ok(_mapper.Map<ExpenseResponse>(expense));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseRequest updateExpenseRequest)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (updateExpenseRequest == null)
                return BadRequest("Invalid expense request.");

            var existingExpense = await _expenseService.GetByIdAsync(id);
            if (existingExpense == null)
                return NotFound();

            var expenseToUpdate = _mapper.Map<Expense>(updateExpenseRequest);
            expenseToUpdate.Id = id;

            var success = await _expenseService.UpdateAsync(expenseToUpdate);
            if (success)
                return Ok("Expense updated successfully.");

            return StatusCode(500, "An error occurred while updating the expense.");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingExpense = await _expenseService.GetByIdAsync(id);
            if (existingExpense == null)
                return NotFound();

            var success = await _expenseService.DeleteAsync(id);
            if (success)
                return Ok("Expense deleted successfully.");

            return StatusCode(500, "An error occurred while deleting the expense.");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpPost("filter/date")]
    public async Task<IActionResult> GetExpensesByDate([FromBody] DateRangeRequest dateRangeRequest)
    {
        try
        {
            var expenses = await _expenseService.GetUserExpensesByDateRangeAsync(dateRangeRequest.UserId, dateRangeRequest.StartDate, dateRangeRequest.EndDate);
            return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpPost("filter/category")]
    public async Task<IActionResult> GetExpensesByCategory([FromBody] CategoryFilterRequest categoryFilterRequest)
    {
        try
        {
            var expenses = await _expenseService.GetUserExpensesByCategoryAsync(categoryFilterRequest.UserId, categoryFilterRequest.Category);
            return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}


public record DateRangeRequest(Guid UserId, DateTime StartDate, DateTime EndDate);
public record CategoryFilterRequest(Guid UserId, string Category);