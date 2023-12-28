namespace WebApi.Dtos.Builders;

public class AddTransactionRequestBuilder
{
    private readonly AddTransactionRequest _request = new();

    public static AddTransactionRequestBuilder New()
    {
        return new();
    }

    public AddTransactionRequestBuilder WithInvoiceDate (DateOnly invoiceDate)
    {
        _request.InvoiceDate = invoiceDate;
        return this;
    }
    
    public AddTransactionRequestBuilder WithDescription (string description)
    {
        _request.Description = description;
        return this;
    }
    
    public AddTransactionRequestBuilder WithInvoiceValue (decimal invoiceValue)
    {
        _request.InvoiceValue = invoiceValue;
        return this;
    }
    
    public AddTransactionRequestBuilder WithCategoryId (Guid categoryId)
    {
        _request.CategoryId = categoryId;
        return this;
    }
    
    public AddTransactionRequestBuilder WithPeriodId(Guid periodId) 
    {
        _request.PeriodId = periodId;
        return this;
    }

    public AddTransactionRequest Build()
    {
        return _request;
    }
}
