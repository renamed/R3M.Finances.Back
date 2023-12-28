using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Context;
using WebApi.Exceptions;
using WebApi.Extensions;
using WebApi.Model;

namespace WebApi.Services;

public class PeriodsService : IPeriodsService
{
    private readonly FinancesContext _financesContext;
    private readonly IMapper _mapper;

    public PeriodsService(FinancesContext financesContext, IMapper mapper)
    {
        _financesContext = financesContext;
        _mapper = mapper;
    }

    public Task<List<Period>> ListAsync()
    {
        return _financesContext.Periods.ToListAsync();
    }

    public Task<Period> GetOneAsync(Guid id)
    {
        return GetOneAsync(new Period { Id = id });
    }

    public Task<Period> GetOneAsync(string name)
    {
        return GetOneAsync(new Period { Name = name });
    }

    public Task<Period> GetOneAsync(Period period)
    {
        Expression<Func<Period, bool>> predicate = BuildPredicate(period);
        return _financesContext.Periods.FirstOrDefaultAsync(predicate);
    }

    public Task<List<Period>> GetAsync(Period period)
    {
        Expression<Func<Period, bool>> predicate = BuildPredicate(period);
        return _financesContext.Periods.Where(predicate).ToListAsync();
    }

    private static Expression<Func<Period, bool>> BuildPredicate(Period period)
    {
        Expression<Func<Period, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(x => x.Id == period.Id, period.Id != default);
        predicate = predicate.AndIf(x => x.Start >= period.Start, period.Start != default);
        predicate = predicate.AndIf(x => x.End <= period.End, period.End != default);
        predicate = predicate.AndIf(x => x.Name == period.Name, period.Name != null);

        return predicate;
    }

    public async Task<Period> AddAsync(Period period)
    {
        if (period.End < period.Start)
        {
            throw new ValidationException("End date before Start");
        }

        var hasOverlappingPeriod = await HasOverlappingPeriodAsync(period.Start, period.End);
        if (hasOverlappingPeriod)
        {
            throw new ValidationException("Overlapping Period already exists");
        }

        var byName = await GetOneAsync(new Period { Name = period.Name });
        if (byName != null)
        {
            throw new ValidationException("Name already exists");
        }

        _financesContext.Periods.Add(period);
        await _financesContext.SaveChangesAsync();
        
        return period;
    }

    public async Task<Period> EditAsync(Guid id, Period period)
    {
        var existingPeriod = await GetOneAsync(new Period { Id = id })
            ?? throw new RecordNotFoundException("Id not found");

        if (period.End < period.Start)
        {
            throw new ValidationException("End date before Start");
        }

        var hasOverlappingPeriod = await HasOverlappingPeriodAsync(period.Start, period.End);
        if (hasOverlappingPeriod)
        {
            throw new ValidationException("Overlapping Period already exists");
        }

        var byName = await GetOneAsync(new Period { Name = period.Name });
        if (byName != null)
        {
            throw new ValidationException("Name already exists");
        }

        _mapper.Map(period, existingPeriod);
        await _financesContext.SaveChangesAsync();

        return existingPeriod;
    }

    public async Task DeleteAsync(Guid id)
    {
        var period = await GetOneAsync(new Period { Id = id })
            ?? throw new RecordNotFoundException("Period not found");

        _financesContext.Periods.Remove(period);
        await _financesContext.SaveChangesAsync();
    }

    private Task<bool> HasOverlappingPeriodAsync(DateOnly start, DateOnly end)
    {
        return _financesContext.Periods.AnyAsync(x => (x.Start >= start && x.End <= end)
                                                 || (x.Start <= start && x.End >= start)
                                                 || (x.Start <= end && x.End >= end));
    }    
}
