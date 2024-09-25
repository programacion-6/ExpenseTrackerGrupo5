using Api.Domain;
using Api.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var expenses = await _expenseService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null)
            return NotFound();

        return Ok(_mapper.Map<ExpenseResponse>(expense));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseRequest updateExpenseRequest)
    {
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var existingExpense = await _expenseService.GetByIdAsync(id);
        if (existingExpense == null)
            return NotFound();

        var success = await _expenseService.DeleteAsync(id);
        if (success)
            return Ok("Expense deleted successfully.");

        return StatusCode(500, "An error occurred while deleting the expense.");
    }

    [HttpPost("filter/date")]
    public async Task<IActionResult> GetExpensesByDate([FromBody] DateRangeRequest dateRangeRequest)
    {
        var expenses = await _expenseService.GetUserExpensesByDateRangeAsync(dateRangeRequest.UserId, dateRangeRequest.StartDate, dateRangeRequest.EndDate);
        return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
    }

    [HttpPost("filter/category")]
    public async Task<IActionResult> GetExpensesByCategory([FromBody] CategoryFilterRequest categoryFilterRequest)
    {
        var expenses = await _expenseService.GetUserExpensesByCategoryAsync(categoryFilterRequest.UserId, categoryFilterRequest.Category);
        return Ok(_mapper.Map<IEnumerable<ExpenseResponse>>(expenses));
    }
}

public record DateRangeRequest(Guid UserId, DateTime StartDate, DateTime EndDate);
public record CategoryFilterRequest(Guid UserId, string Category);
