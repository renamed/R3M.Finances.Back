using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinancialGoalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IFinancialGoalsService _financialGoalsService;

    public FinancialGoalsController(IMapper mapper, IFinancialGoalsService financialGoalsService)
    {
        _mapper = mapper;
        _financialGoalsService = financialGoalsService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var financialGoal = await _financialGoalsService.GetAsync(id)
            ?? throw new RecordNotFoundException("Financial goal not found");
        var response = _mapper.Map<ListFinancialGoalResponse>(financialGoal);
        return Ok(response);
    }

    [HttpGet("period/{id}")]
    public async Task<IActionResult> ListAsync(Guid id)
    {
        var financialGoals = await _financialGoalsService.ListAsync(new FinancialGoal { PeriodId = id });
        var response = financialGoals.Select(_mapper.Map<ListFinancialGoalResponse>);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AddFinancialGoalRequest request)
    {
        var financial = _mapper.Map<FinancialGoal>(request);
        var financialGoalResponse = await _financialGoalsService.AddAsync(financial);
        var response = _mapper.Map<AddFinancialGoalResponse>(financialGoalResponse);
        return Created(Request.Path, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync(Guid id, [FromBody] EditFinancialGoalRequest request)
    {
        var financial = _mapper.Map<FinancialGoal>(request);
        var financialGoalResponse = await _financialGoalsService.EditAsync(id, financial);
        var response = _mapper.Map<EditFinancialGoalResponse>(financialGoalResponse);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _financialGoalsService.DeleteAsync(id);
        return Ok();
    }
}
