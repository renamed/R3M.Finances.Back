using WebApi.Model;

namespace WebApi.Services;

public interface ITransactionService
{
    Task<Transaction> CreateAsync(Transaction transaction);
    decimal GetBalance();
    Task<IEnumerable<Transaction>> ListByPeriodAsync(Guid id);
    Task<IEnumerable<Transaction>> ListByPeriodAsync(string name);
}