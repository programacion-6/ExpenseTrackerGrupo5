using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/recurrentexpenses")]
public class RecurrentExpensesController : ControllerBase
{
    private readonly IMapper _mapper;

    public RecurrentExpensesController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecurrentExpense([FromBody] CreateRecurrentExpenseRequest createRecurrentExpenseRequest)
    {
        return Ok("Recurrent expense created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRecurrentExpenses()
    {
        return Ok("recurrent Expenses");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecurrentExpenseById(Guid id)
    {
        return Ok("recurrent Expense");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecurrentExpense(Guid id, [FromBody] UpdateRecurrentExpenseRequest updateRecurrentExpenseRequest)
    {
        return Ok("Recurrent expense updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecurrentExpense(Guid id)
    {
        return Ok("Recurrent expense deleted successfully.");
    }

    [HttpPost("monthlycalculation")]
    public async Task<IActionResult> LogMonthlyRecurrentExpenses()
    {
        return Ok("Monthly recurrent expenses logged successfully.");
    }
}
