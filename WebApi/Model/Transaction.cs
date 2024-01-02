namespace WebApi.Model;

public class Transaction : Register
{
    public DateOnly InvoiceDate { get; set; }
    public string Description { get; set; }
    public decimal? InvoiceValue { get; set; }
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }

    public Period Period { get; set; }
    public Guid PeriodId { get; set; }

    public IEnumerable<TransactionPart> Parts { get; set; }
}
