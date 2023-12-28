using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Exceptions;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeriodsController : ControllerBase
{        
    private readonly IMapper _mapper;
    private readonly IPeriodsService _periodsService;

    public PeriodsController(IMapper mapper, IPeriodsService periodsService)
    {
        _mapper = mapper;
        _periodsService = periodsService;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var periods = await _periodsService.ListAsync();
        return Ok(periods.Select(_mapper.Map<ListPeriodsResponse>));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var period = await _periodsService.GetOneAsync(id)
            ?? throw new RecordNotFoundException("Period not found");

        return Ok(_mapper.Map<ListPeriodsResponse>(period));
    }

    [HttpGet("date/{start}")]
    public async Task<IActionResult> GetAsync(DateOnly start, [FromQuery]DateOnly? end)
    {
        var periods = await _periodsService.GetAsync(new Period { Start = start, End = end ?? default });
        return Ok(periods.Select(_mapper.Map<ListPeriodsResponse>));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] AddPeriodRequest request)
    {
        var periodRequest = _mapper.Map<Period>(request);
        var newPeriod = await _periodsService.AddAsync(periodRequest);
        var periodResponse = _mapper.Map<AddPeriodResponse>(newPeriod);
        return Created(Request.Path, periodResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync(Guid id, [FromBody] EditPeriodRequest request)
    {
        var periodRequest = _mapper.Map<Period>(request);
        var newPeriod = await _periodsService.EditAsync(id, periodRequest);
        var periodResponse = _mapper.Map<EditPeriodResponse>(newPeriod);
        return Ok(periodResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _periodsService.DeleteAsync(id);
        return Ok();
    }
}