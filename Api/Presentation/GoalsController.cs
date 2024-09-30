using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Api.Domain.Services;

using System.Security.Claims;

[ApiController]
[Route("api/goals")]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IGoalService _goalService;

    public GoalsController(IGoalService goalService ,IMapper mapper)
    {
        _goalService = goalService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest createGoalRequest)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var goal = _mapper.Map<Goal>(createGoalRequest);
        goal.user_id = userId;

        var result = await _goalService.CreateAsync(goal);
        if(result)
        {
            return Ok("Goal created successfully.");
        }
        return StatusCode(500, "Error saving goal.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals()
    {
        var goals = await _goalService.GetAllAsync();
        return Ok(goals);
    }

    [HttpGet("actives")]
    public async Task<IActionResult> GetActiveGoals()
    {
        return Ok("activeGoals");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById(Guid id)
    {
        return Ok("goal");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        return Ok($"Goal with ID {id} deleted successfully.");
    }
}
