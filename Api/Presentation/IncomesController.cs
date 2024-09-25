using System.Security.Claims;

using Api.Domain;
using Api.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/incomes")]
[Authorize]
public class IncomesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IIncomeService _incomeService;

    public IncomesController(IMapper mapper, IIncomeService incomeService)
    {
        _mapper = mapper;
        _incomeService = incomeService;
    }

    [HttpPost]
    public async Task<IActionResult> LogIncome([FromBody] CreateIncomeRequest createIncomeRequest)
    {
        if (createIncomeRequest == null)
            return BadRequest("Invalid income request.");

        try
        {
            var income = _mapper.Map<Income>(createIncomeRequest);
            income.Id = Guid.NewGuid();
            income.CreatedAt = DateTime.UtcNow;

            var success = await _incomeService.CreateAsync(income);
            if (success)
                return CreatedAtAction(nameof(GetIncomeById), new { id = income.Id }, _mapper.Map<IncomeResponse>(income));

            return StatusCode(500, "An error occurred while logging the income.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIncomes()
    {
        try
        {
            var incomes = await _incomeService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<IncomeResponse>>(incomes));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeById(Guid id)
    {
        try
        {
            var income = await _incomeService.GetByIdAsync(id);
            if (income == null)
                return NotFound();

            return Ok(_mapper.Map<IncomeResponse>(income));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeRequest updateIncomeRequest)
    {
        if (updateIncomeRequest == null)
            return BadRequest("Invalid income request.");

        try
        {
            var existingIncome = await _incomeService.GetByIdAsync(id);
            if (existingIncome == null)
                return NotFound();

            var incomeToUpdate = _mapper.Map<Income>(updateIncomeRequest);
            incomeToUpdate.Id = id; 

            var success = await _incomeService.UpdateAsync(incomeToUpdate);
            if (success)
                return Ok("Income updated successfully.");

            return StatusCode(500, "An error occurred while updating the income.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        try
        {
            var existingIncome = await _incomeService.GetByIdAsync(id);
            if (existingIncome == null)
                return NotFound();

            var success = await _incomeService.DeleteAsync(id);
            if (success)
                return Ok("Income deleted successfully.");

            return StatusCode(500, "An error occurred while deleting the income.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
