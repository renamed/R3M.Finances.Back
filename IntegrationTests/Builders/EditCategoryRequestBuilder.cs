using WebApi.Dtos;
using WebApi.Model;

namespace IntegrationTests.Builders;

public class EditCategoryRequestBuilder
{
    private EditCategoryRequest request = new();

    public static EditCategoryRequestBuilder New()
        => new();

    public EditCategoryRequestBuilder WithName(string name)
    {
        request.Name = name;
        return this;
    }

    public EditCategoryRequestBuilder WithTransactionType(TransactionType? transactionType)
    {
        request.TransactionType = transactionType;
        return this;
    }

    public EditCategoryRequest Build()
    {
        return request;
    }
}
