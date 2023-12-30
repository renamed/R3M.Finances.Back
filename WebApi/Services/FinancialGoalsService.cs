using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using WebApi.Context;
using WebApi.Exceptions;
using WebApi.Extensions;
using WebApi.Model;

namespace WebApi.Services;

public class FinancialGoalsService : IFinancialGoalsService
{
    private readonly FinancesContext _financesContext;
    private readonly IMapper _mapper;
    private readonly ICategoriesService _categoryService;
    private readonly IPeriodsService _periodsService;

    public FinancialGoalsService(FinancesContext financesContext, IMapper mapper, ICategoriesService categoryService, IPeriodsService periodsService)
    {
        _financesContext = financesContext;
        _mapper = mapper;
        _categoryService = categoryService;
        _periodsService = periodsService;
    }

    public Task<List<FinancialGoal>> ListAsync(FinancialGoal financialGoal)
    {
        var predicate = GetPredicate(financialGoal);
        return GetBaseSelectFrom().Where(predicate).ToListAsync();
    }

    public Task<FinancialGoal> GetAsync(FinancialGoal financialGoal)
    {
        var predicate = GetPredicate(financialGoal);
        return GetBaseSelectFrom().FirstOrDefaultAsync(predicate);
    }

    public Task<FinancialGoal> GetAsync(Guid id)
    {
        return GetAsync(new FinancialGoal { Id = id });
    }

    public async Task<FinancialGoal> AddAsync(FinancialGoal financialGoal)
    {
        await AddOrEditValidationsAsync(financialGoal);

        _financesContext.Add(financialGoal);
        await _financesContext.SaveChangesAsync();

        return financialGoal;
    }

    public async Task<FinancialGoal> EditAsync(Guid id, FinancialGoal financialGoal)
    {
        var existingFinancialGoal = await GetAsync(id)
            ?? throw new RecordNotFoundException("Financial goal not found");

        await AddOrEditValidationsAsync(existingFinancialGoal);

        _mapper.Map(financialGoal, existingFinancialGoal);
        await _financesContext.SaveChangesAsync();

        return existingFinancialGoal;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingFinancialGoal = await GetAsync(id)
            ?? throw new RecordNotFoundException("Financial goal not found");

        _financesContext.FinancialGoals.Remove(existingFinancialGoal);
        await _financesContext.SaveChangesAsync();
    }

    private IIncludableQueryable<FinancialGoal, Category> GetBaseSelectFrom()
    {
        return _financesContext
                        .FinancialGoals
                        .Include(x => x.Period)
                        .Include(x => x.Category);
    }

    private async Task AddOrEditValidationsAsync(FinancialGoal financialGoal)
    {
        var category = await _categoryService.GetAsync(financialGoal.CategoryId)
            ?? throw new RecordNotFoundException("Category not found");

        _ = await _periodsService.GetOneAsync(financialGoal.PeriodId)
            ?? throw new RecordNotFoundException("Period not found");
        
        if (financialGoal.Goal < 0 && category.TransactionType != TransactionType.Debit)
        {
            throw new ValidationException("Negative value for Goal requires 'Debit' transaction type");
        }

        if (financialGoal.Goal > 0 && category.TransactionType != TransactionType.Credit)
        {
            throw new ValidationException("Positive value for Goal requires 'Credit' transaction type");
        }
    }

    private static Expression<Func<FinancialGoal, bool>> GetPredicate(FinancialGoal financialGoal)
    {
        Expression<Func<FinancialGoal, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(f => f.PeriodId == financialGoal.PeriodId, financialGoal.PeriodId != default);
        predicate = predicate.AndIf(f => f.CategoryId == financialGoal.CategoryId, financialGoal.CategoryId != default);
        predicate = predicate.AndIf(f => f.Id == financialGoal.Id, financialGoal.Id != default);

        return predicate;
    }
}
