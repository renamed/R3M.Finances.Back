using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
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

    public async Task<IEnumerable<Transaction>> ListByPeriodAsync(Guid id)
    {
        return await ListAsync((x) => x.PeriodId == id);
    }

    public async Task<IEnumerable<Transaction>> ListByPeriodAsync(string name)
    {
        return await ListAsync((x) => x.Period.Name == name);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var category = await _categoriesService.GetAsync(transaction.CategoryId)
            ?? throw new ValidationException("Category does not exist");

        var period = await _periodsService.GetOneAsync(transaction.PeriodId)
            ?? throw new ValidationException("Period does not exist");

        transaction.CategoryId = category.Id;
        transaction.Category = category;
        transaction.PeriodId = period.Id;
        transaction.Period = period;

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

        await ValidatePartsAsync(transaction);
        

        _financesContext.Transactions.Add(transaction);

        foreach (var part in transaction.Parts)
        {
            part.TransactionId = transaction.Id;
        }
        await _financesContext.SaveChangesAsync();

        return transaction;
    }


    private async Task ValidatePartsAsync(Transaction transaction)
    {
        if (transaction.Parts == null) return;
        if (!transaction.Parts.Any()) return;

        var partsList = transaction.Parts.ToList();

        var sum = partsList.Sum(x => x.Value);
        if (sum != transaction.InvoiceValue)
        {
            throw new ValidationException("Parts sum value do not match transaction value");
        }

        foreach(var a in partsList)
        {
            if (!a.CategoryId.HasValue) return;

            if (a.Value < 0 && transaction.Category.TransactionType == TransactionType.Credit)
            {
                throw new ValidationException("Transaction part value cannot be negative with Credit transaction type");
            }

            if (a.Value > 0 && transaction.Category.TransactionType == TransactionType.Debit)
            {
                throw new ValidationException("Transaction part value cannot be positive with Debit transaction type");
            }

            var category = await _categoriesService.GetAsync(a.CategoryId.Value) 
                ?? throw new RecordNotFoundException("Category not found for transaction part");

            if (a.Value < 0 && category.TransactionType == TransactionType.Credit)
            {
                throw new ValidationException("Transaction part value cannot be negative with Credit transaction type");
            }

            if (a.Value > 0 && category.TransactionType == TransactionType.Debit)
            {
                throw new ValidationException("Transaction part value cannot be positive with Debit transaction type");
            }
        }
    }

    private async Task<List<Transaction>> ListAsync(Func<Transaction, bool> predicate)
    {
        var transactions = _financesContext.Transactions
                .Include(x => x.Period)
                .Include(x => x.Category)
                .Where(predicate)
                .ToList();

        foreach (var transaction in transactions)
        {
            transaction.Parts = await 
                _financesContext
                .TransactionParts
                .Where(x => x.Id == transaction.Id)
                .ToListAsync();
        }

        return transactions;
    }
}
