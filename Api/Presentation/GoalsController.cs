using Api.Domain;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/goals")]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IMapper _mapper;

    public GoalsController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest createGoalRequest)
    {
        return Ok("Goal created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals()
    {
        return Ok("goals");
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
