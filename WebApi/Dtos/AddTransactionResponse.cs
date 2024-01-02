namespace WebApi.Dtos;

public class AddTransactionResponse
{
    public Guid Id { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string Description { get; set; }
    public decimal InvoiceValue { get; set; }
    public DefaultCategoryResponse Category { get; set; }
    public DefaultPeriodResponse Period { get; set; }
    public IEnumerable<AddTransactionPartResponse> Parts { get; set; }
}

public class AddTransactionPartResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DefaultCategoryResponse Category { get; set; }
}