namespace WebApi.Dtos;

public class ListTransactionsResponse
{
    public decimal Balance { get; set; }
    public IEnumerable<ListTransactionsNodeResponse> Transactions { get; set; }
    public decimal PeriodBalance { get; internal set; }
}

public class ListTransactionsNodeResponse
{
    public Guid Id { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string Description { get; set; }
    public decimal InvoiceValue { get; set; }
    public DefaultCategoryResponse Category { get; set; }
    public DefaultPeriodResponse Period { get; set; }

    public IEnumerable<ListTransactionPartResponse> Parts { get; set; }
}

public class ListTransactionPartResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DefaultCategoryResponse Category { get; set; }
}