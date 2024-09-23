using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IMapper _mapper;

    public ExpensesController(IMapper mapper)
    {
        _mapper = mapper;
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
        return Ok("Expense updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        return Ok("Expense deleted successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest)
    {
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
