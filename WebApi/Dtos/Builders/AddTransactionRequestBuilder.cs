using WebApi.Model;

namespace WebApi.Dtos.Builders;

public class AddTransactionRequestBuilder
{
    private readonly AddTransactionRequest _request = new();
    private readonly List<AddTransactionPartRequest> _parts = new();

    public static AddTransactionRequestBuilder New()
    {
        return new();
    }

    public AddTransactionRequestBuilder WithInvoiceDate(DateOnly invoiceDate)
    {
        _request.InvoiceDate = invoiceDate;
        return this;
    }

    public AddTransactionRequestBuilder WithDescription(string description)
    {
        _request.Description = description;
        return this;
    }

    public AddTransactionRequestBuilder WithInvoiceValue(decimal invoiceValue)
    {
        _request.InvoiceValue = invoiceValue;
        return this;
    }

    public AddTransactionRequestBuilder WithCategoryId(Guid categoryId)
    {
        _request.CategoryId = categoryId;
        return this;
    }

    public AddTransactionRequestBuilder WithPeriodId(Guid periodId)
    {
        _request.PeriodId = periodId;
        return this;
    }

    public AddTransactionRequestBuilder AddPart(AddTransactionPartRequest part)
    {
        _parts.Add(part);
        return this;
    }

    public AddTransactionRequest Build()
    {
        _request.Parts = _parts.Count == 0 ? null : _parts;
        return _request;
    }
}

public class AddTransactionPartRequestBuilder
{
    private readonly AddTransactionPartRequest _request = new();
    public static AddTransactionPartRequestBuilder New => new();

    public AddTransactionPartRequestBuilder WithDescription(string description)
    {
        _request.Description = description;
        return this;
    }
    public AddTransactionPartRequestBuilder WithValue(decimal value)
    {
        _request.Value = value;
        return this;
    }
    public AddTransactionPartRequestBuilder WithCategoryId(Guid? categoryId)
    {
        _request.CategoryId = categoryId;
        return this;
    }
    public AddTransactionPartRequest Build() => _request;
}