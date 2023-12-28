using WebApi.Dtos;
using WebApi.Model;

namespace IntegrationTests.Builders;

public class AddCategoryRequestBuilder
{
    private AddCategoryRequest request = new();

    public static AddCategoryRequestBuilder New()
        => new();

    public AddCategoryRequestBuilder WithName(string name)
    {
        request.Name = name;
        return this;
    }

    public AddCategoryRequestBuilder WithTransactionType(TransactionType transactionType)
    {
        request.TransactionType = transactionType;
        return this;
    }

    public AddCategoryRequestBuilder WithParentId(Guid id)
    {
        request.ParentId = id;
        return this;
    }

    public AddCategoryRequest Build()
    {
        return request;
    }
}
