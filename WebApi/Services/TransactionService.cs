using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Exceptions;
using WebApi.Model;

namespace WebApi.Services;

public class TransactionService : ITransactionService
{
    private readonly FinancesContext _financesContext;
    private readonly IMapper _mapper;

    private readonly ICategoriesService _categoriesService;
    private readonly IPeriodsService _periodsService;

    public TransactionService(IMapper mapper, FinancesContext financesContext, ICategoriesService categoriesService, IPeriodsService periodsService)
    {
        _mapper = mapper;
        _financesContext = financesContext;
        _categoriesService = categoriesService;
        _periodsService = periodsService;
    }

    public decimal GetBalance()
    {
        return (decimal)_financesContext.Transactions.Sum(x => (double)x.InvoiceValue);
    }

    public IEnumerable<Transaction> ListByPeriod(Guid id)
    {
        return List((x) => x.PeriodId == id);
    }

    public IEnumerable<Transaction> ListByPeriod(string name)
    {
        return List((x) => x.Period.Name == name);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {            
        var category = await _categoriesService.GetAsync(transaction.CategoryId)
            ?? throw new ValidationException("Category does not exist");

        var period = await _periodsService.GetOneAsync(transaction.PeriodId)
            ?? throw new ValidationException("Period does not exist");

        if (transaction.InvoiceDate < period.Start || transaction.InvoiceDate > period.End)
        {
            throw new ValidationException("Invoice date does not match period range");
        }

        if (transaction.InvoiceValue < 0 && category.TransactionType == TransactionType.Credit)
        {
            throw new ValidationException("Invoice value cannot be negative with Credit transaction type");
        }

        if (transaction.InvoiceValue > 0 && category.TransactionType == TransactionType.Debit)
        {
            throw new ValidationException("Invoice value cannot be positive with Debit transaction type");
        }

        transaction.Category = category;
        transaction.CategoryId = category.Id;

        transaction.Period = period;
        transaction.PeriodId = period.Id;

        _financesContext.Transactions.Add(transaction);
        await _financesContext.SaveChangesAsync();

        return transaction;
    }

    private IEnumerable<Transaction> List(Func<Transaction, bool> predicate)
    {
        return _financesContext.Transactions
                .Include(x => x.Period)
                .Include(x => x.Category)
                .Where(predicate);
    }
}
