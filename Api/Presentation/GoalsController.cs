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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalRequest updateGoalRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var goal = await _goalService.GetByIdAsync(id);

        if (goal == null || goal.user_id != Guid.Parse(userId))
        {
            return NotFound("Goal not found or you do not have permission to update this goal.");
        }
        
        goal.Currency = updateGoalRequest.Currency;
        goal.goal_amount = updateGoalRequest.goal_amount;
        goal.Deadline = updateGoalRequest.Deadline;

        var result = await _goalService.UpdateAsync(goal);

        if (result)
        {
            return Ok("Goal updated successfully.");
        }

        return StatusCode(500, "Error updating goal.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var goals = await _goalService.GetGoalsByUserId(Guid.Parse(userId));

        if (goals == null || !goals.Any())
        {
            return NotFound("No goals found for the current user.");
        }

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
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var goal = await _goalService.GetByIdAsync(id);
        if (goal == null || goal.user_id != userId)
        {
            return NotFound($"Goal with ID {id} not found.");
        }
        return Ok(goal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var goal = await _goalService.GetByIdAsync(id);
        if (goal == null)
        {
            return NotFound($"Goal with ID {id} not found.");
        }
        var result = await _goalService.DeleteAsync(id);
        if (result){
            return Ok ("Goal deleted successfully.");
        }
        return StatusCode(500, "Error deleting goal.");
    }
}
