using System.Security.Claims;

using Api.Domain;
using Api.Domain.Services;

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
    private readonly IExpenseService _expenseService;

    public ExpensesController(IMapper mapper, ITracker<Expense, Budget> tracker, IExpenseService expenseService)
    {
        _mapper = mapper;
        _tracker = tracker;
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userId is null || userEmail is null)
            {
                return BadRequest("User not found");
            }

            var expense = _mapper.Map<Expense>(createExpenseRequest);
            expense.UserId = Guid.Parse(userId);

            var success = await _expenseService.CreateAsync(expense);

            if (!success)
            {
                return StatusCode(500, "An error occurred while creating the expense.");
            }

            try
            {
                await _tracker.TrackNewUserEntry(expense, userEmail);
            }
            catch (Exception exception)
            {
                return StatusCode(400, $"{exception.Message}");
            }

            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, _mapper.Map<ExpenseResponse>(expense));

        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByUserAsync()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expenses = await _expenseService.GetAllByUserAsync(userId);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expenseUserId = await _expenseService.GetUserIdByExpenseId(id);
            
            if (expenseUserId == null)
                return NotFound("Expense not found ");
            if (expenseUserId.ToString() != userId)
                return NotFound("You do not have permission to view this expense.");
            
            var expense = await _expenseService.GetByIdAsync(id);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userId is null || userEmail is null)
            {
                return BadRequest("User not found");
            }
            
            var expenseUserId = await _expenseService.GetUserIdByExpenseId(id);
            var expense = await _expenseService.GetByIdAsync(id);
            
            if (expense == null || expenseUserId != Guid.Parse(userId))
                return NotFound("Income not found or you do not have permission to update this income."); 

            var expenseToUpdate = _mapper.Map<Expense>(updateExpenseRequest);
            expenseToUpdate.Id = id;
            expenseToUpdate.UserId = Guid.Parse(userId);
            
            var result = await _expenseService.UpdateAsync(expenseToUpdate);
            
            if (!result)
            {
                return StatusCode(500, "An error occurred while updating the expense.");
            }
            
            try
            {
                await _tracker.TrackUpdatedUserEntry(expense, expenseToUpdate, userEmail);
            }
            catch (Exception exception)
            {
                return StatusCode(400, $"{exception.Message}");
            }

            return Ok("Expense updated successfully.");
            
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userId is null || userEmail is null)
            {
                return BadRequest("User not found");
            }

            var expenseUserId = await _expenseService.GetUserIdByExpenseId(id);
            var existingExpense = await _expenseService.GetByIdAsync(id);

            if (existingExpense == null)
                return NotFound("Expense not found");

            if (expenseUserId != Guid.Parse(userId))
                return NotFound("You do not have permission to delete this expense.");
            var success = await _expenseService.DeleteAsync(id);

            if (!success)
            {
                return StatusCode(500, "An error occurred while deleting the expense.");
            }

            try
            {
                await _tracker.TrackDeletedUserEntry(existingExpense, userEmail);
            }
            catch (Exception exception)
            {
                return StatusCode(400, $"{exception.Message}");
            }

            return Ok("Expense deleted successfully.");

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
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (dateRangeRequest.UserId != userId)
                return Unauthorized("You do not have permission to filter expenses for another user.");

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
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (categoryFilterRequest.UserId != userId)
                return Unauthorized("You do not have permission to filter expenses for another user.");

            var expenses = await _expenseService.GetUserExpensesByCategoryAsync(categoryFilterRequest.UserId, categoryFilterRequest.Category);
            return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
    
    [HttpGet("user/{expenseId}")]
    public async Task<IActionResult> GetUserIdByExpenseId(Guid expenseId)
    {
        try
        {
            var userId = await _expenseService.GetUserIdByExpenseId(expenseId);
        
            if (userId == null)
                return NotFound("Expense not found");

            return Ok(userId);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }

}
