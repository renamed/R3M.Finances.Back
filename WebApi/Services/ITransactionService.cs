using WebApi.Model;

namespace WebApi.Services;

public interface ITransactionService
{
    Task<Transaction> CreateAsync(Transaction transaction);
    decimal GetBalance();
    IEnumerable<Transaction> ListByPeriod(Guid id);
    IEnumerable<Transaction> ListByPeriod(string name);
}