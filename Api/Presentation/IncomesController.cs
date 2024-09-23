using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/incomes")]
public class IncomesController : ControllerBase
{
    private readonly IMapper _mapper;

    public IncomesController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> LogIncome([FromBody] CreateIncomeRequest createIncomeRequest)
    {
        return Ok("Income logged successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIncomes()
    {
        return Ok("incomes");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeById(Guid id)
    {
        return Ok("income");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeRequest updateIncomeRequest)
    {
        return Ok("Income updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        return Ok("Income deleted successfully.");
    }
}
