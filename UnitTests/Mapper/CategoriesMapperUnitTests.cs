using AutoMapper;
using FluentAssertions;
using WebApi.Dtos;
using WebApi.Model;
using WebApi.Model.Builders;

namespace UnitTests.Mapper;

public class CategoriesMapperUnitTests
{
    private readonly IMapper _mapper;

    public CategoriesMapperUnitTests()
    {
        _mapper = MapperFactory.GetCategoriesMapper();
    }

    [Fact]
    public void Category_To_ListCategoriesResponse_ShouldMapId()
    {
        var category = CategoryBuilder
            .New
            .WithId(Guid.NewGuid())
            .Build();

        var mapped = _mapper.Map<ListCategoriesResponse>(category);

        mapped.Id.Should().NotBeEmpty();
        mapped.Id.Should().Be(category.Id);
    }

    [Theory]
    [InlineData(TransactionType.Unknown)]
    [InlineData(TransactionType.Credit)]
    [InlineData(TransactionType.Debit)]
    public void Category_To_ListCategoriesResponse_ShouldMapTransactionType(TransactionType transactionType)
    {
        var category = CategoryBuilder
            .New
            .WithTransactionType(transactionType)                
            .Build();

        var mapped = _mapper.Map<ListCategoriesResponse>(category);

        mapped.TransactionType.Should().Be(transactionType);
    }

    [Fact]
    public void Category_To_ListCategoriesResponse_ShouldMapName()
    {
        var category = CategoryBuilder
            .New
            .WithName("1234321")
            .Build();

        var mapped = _mapper.Map<ListCategoriesResponse>(category);

        mapped.Name.Should().Be(category.Name);
    }
}
