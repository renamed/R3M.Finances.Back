using WebApi.Model;

namespace WebApi.Services;

public interface IFinancialGoalsService
{
    Task<FinancialGoal> AddAsync(FinancialGoal financialGoal);
    Task DeleteAsync(Guid id);
    Task<FinancialGoal> EditAsync(Guid id, FinancialGoal financialGoal);
    Task<FinancialGoal> GetAsync(FinancialGoal financialGoal);
    Task<FinancialGoal> GetAsync(Guid id);
    Task<List<FinancialGoal>> ListAsync(FinancialGoal financialGoal);
}