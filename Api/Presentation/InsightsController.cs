using System.Security.Claims;

using Api.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/summary")]
[Authorize]
public class InsightsController : ControllerBase
{
    private readonly IReportService _reportService;

    public InsightsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("monthlyexpenses")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return BadRequest("User not found");
        }

        try
        {
            var guidUserId = Guid.Parse(userId);
            var monthlySummary = await _reportService.GetUserMonthlySummary(guidUserId);

            return Ok(monthlySummary);
        }
        catch (Exception exception)
        {
            return StatusCode(500, $"Internal server error: {exception.Message}");
        }
    }

    [HttpGet("expenseinsights")]
    public async Task<IActionResult> GetExpenseInsights()
    {

        return Ok("expense insights");
    }
}
