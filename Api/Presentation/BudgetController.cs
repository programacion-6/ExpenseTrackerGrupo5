using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IMapper _mapper;

    public BudgetController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> SetMonthlyBudget([FromBody] CreateBudgetRequest createBudgetRequest)
    {
        return Ok("Budget set successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentMonthBudget()
    {
        return Ok("budget");
    }
}
