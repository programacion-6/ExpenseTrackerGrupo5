using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/summary")]
[Authorize]
public class InsightsController : ControllerBase
{
    [HttpGet("monthlyexpenses")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        return Ok("monthlyexpense");
    }

    [HttpGet("expenseinsights")]
    public async Task<IActionResult> GetExpenseInsights()
    {

        return Ok("expense insights");
    }
}
