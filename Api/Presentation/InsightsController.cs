using System.Security.Claims;

using Api.Application;

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
    public async Task<IActionResult> GetMonthlySummary([FromQuery] DateTime? month)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return BadRequest("User not found");
        }

        var summaryDate = month ?? DateTime.Today;

        if (DateChecker.IsGreaterThanThisMonth(summaryDate))
        {
            return BadRequest("The date must be less than or equal to the current month and year.");
        }

        try
        {
            var guidUserId = Guid.Parse(userId);
            var monthlySummary = await _reportService.GetUserMonthlySummary(guidUserId, summaryDate);

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
