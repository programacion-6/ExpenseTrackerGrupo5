using System.Security.Claims;

using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/expenses")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITracker<Expense, Budget> _tracker;

    public ExpensesController(IMapper mapper, ITracker<Expense, Budget> tracker)
    {
        _mapper = mapper;
        _tracker = tracker;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        return Ok("expenses");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        return Ok("some expense");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseRequest updateExpenseRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var oldExpense = new Expense();
        var newExpense = _mapper.Map<Expense>(updateExpenseRequest);

        try
        {
            await _tracker.TrackUpdatedUserEntry(oldExpense, newExpense, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok("Expense updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var expense = new Expense();

        try
        {
            await _tracker.TrackDeletedUserEntry(expense, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok("Expense deleted successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null)
        {
            return BadRequest("User not found");
        }

        var expense = _mapper.Map<Expense>(createExpenseRequest);
        expense.UserId = Guid.Parse(userId);

        try
        {
            await _tracker.TrackNewUserEntry(expense, userEmail);
        }
        catch (Exception exception)
        {
            return StatusCode(400, $"{exception.Message}");
        }

        return Ok("Expense created successfully.");
    }

    [HttpPost("filter/date")]
    public async Task<IActionResult> GetExpensesByDate(DateTime startDate, DateTime endDate)
    {
        return Ok("filtered Expenses");
    }

    [HttpPost("filter/category")]
    public async Task<IActionResult> GetExpensesByCategory(string category)
    {
        return Ok("filtered Expenses");
    }
}
