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
            expense.UserId = userId;  // Asignar el UserId al gasto
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
            Console.WriteLine("***************************");
            Console.WriteLine(userId);
            Console.WriteLine(expenseUserId);
            Console.WriteLine("***************************");
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
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var expenseUserId = await _expenseService.GetUserIdByExpenseId(id);

            if (expenseUserId == null)
                return NotFound("Expense not found.");
        
            if (expenseUserId != userId)
                return Forbid("You do not have permission to update this expense.");

            var existingExpense = await _expenseService.GetByIdAsync(id);
            if (existingExpense == null)
                return NotFound("Expense not found.");

            var expenseToUpdate = _mapper.Map(updateExpenseRequest, existingExpense);
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
            var expenseUserId = await _expenseService.GetUserIdByExpenseId(id);
            var existingExpense = await _expenseService.GetByIdAsync(id);

            if (existingExpense == null)
                return NotFound("Expense not found");

            if (expenseUserId != userId)
                return NotFound("You do not have permission to delete this expense.");
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
