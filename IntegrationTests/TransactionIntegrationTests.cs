using FluentAssertions;
using IntegrationTests.Config;
using System.Net;
using WebApi;
using WebApi.Dtos;
using WebApi.Dtos.Builders;
using Xunit;

namespace IntegrationTests;

public class TransactionIntegrationTests(CustomWebApplicationFactory<Program> applicationFactory) : IntegrationTestsBase(applicationFactory)
{
    private const string ListTransactionByPeriodNameUrl = "/api/transaction/period/name/";
    private const string ListTransactionByPeriodIdUrl = "/api/transaction/period/";
    private const string AddCategoryUrl = "/api/transaction";

    #region GET

    [Fact]
    public async Task ListAsync_Name_ShouldReturn200_WhenNameIsProvided()
    {
        var url = ListTransactionByPeriodNameUrl + "202401";

        var response = await GetAsync<ListCategoriesResponse>(url);

        response.Response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListAsync_Name_ShouldReturnAllTransactions_WhenNameIsProvided()
    {
        var url = ListTransactionByPeriodNameUrl + "202401";

        var response = await GetAsync<ListTransactionsResponse>(url);

        response.Body.Transactions.Should().HaveCount(99);
        response.Body.Transactions.Should().AllSatisfy(a => a.Period.Name.Should().Be("202401"));
        response.Body.Transactions.Should().AllSatisfy(a => a.Parts.Should().BeEmpty());
    }

    [Fact]
    public async Task ListAsync_Name_ShouldReturnBalance_WhenNameIsProvided()
    {
        var url = ListTransactionByPeriodNameUrl + "202311";

        var response = await GetAsync<ListTransactionsResponse>(url);

        response.Body.Balance.Should().BeApproximately(3838.07m, 0.01m);
    }

    [Fact]
    public async Task ListAsync_Id_ShouldReturn200_WhenIdIsProvided()
    {
        var url = ListTransactionByPeriodIdUrl + "f0028ad8-e768-44f1-8798-f97e7b6bd7bf";

        var response = await GetAsync<ListCategoriesResponse>(url);

        response.Response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListAsync_Id_ShouldReturnAllTransactions_WhenIdIsProvided()
    {
        var url = ListTransactionByPeriodIdUrl + "f0028ad8-e768-44f1-8798-f97e7b6bd7bf";

        var response = await GetAsync<ListTransactionsResponse>(url);

        response.Body.Transactions.Should().HaveCount(47);
        response.Body.Transactions.Should().AllSatisfy(a => a.Period.Name.Should().Be("202402"));
    }

    [Fact]
    public async Task ListAsync_Id_ShouldReturnBalance_WhenIdIsProvided()
    {
        var url = ListTransactionByPeriodIdUrl + "7f565395-7c03-4519-8ead-1b5685e9716d";

        var response = await GetAsync<ListTransactionsResponse>(url);

        response.Body.Balance.Should().BeApproximately(3838.07m, 0.01m);
    }

    #endregion

    #region POST
    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenCategoryDoesNotExist()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.NewGuid())
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("Transaction Description")
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenCategoryDoesNotExist()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.NewGuid())
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("Transaction Description")
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Category does not exist");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenPeriodDoesNotExist()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.NewGuid())
            .WithDescription("Transaction Description")
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenPeriodDoesNotExist()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.NewGuid())
            .WithDescription("Transaction Description")
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Period does not exist");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenDescriptionIsNull()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription(null)
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenDescriptionIsLessThan5Chars()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("1234")
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenDescriptionIsMoreThan250()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription(string.Join(string.Empty, Enumerable.Repeat("a", 251)))
            .WithInvoiceDate(new DateOnly(2023, 11, 05))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenInvoiceDateIsLessThanPeriodRange()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 10, 31))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceDateIsLessThanPeriodRange()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 10, 31))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Invoice date does not match period range");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenInvoiceDateIsGreaterThanPeriodRange()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 12, 1))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceDateIsGreaterThanPeriodRange()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 12, 1))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Invoice date does not match period range");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn201_WhenTransactionIsAdded()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .Build();

        var response = await PostAsync<AddTransactionResponse, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnObject_WhenTransactionIsAdded()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("2c47651d-b765-4e1a-8409-9a78bfc3e23d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2024, 1, 13))
            .Build();

        var response = await PostAsync<AddTransactionResponse, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Id.Should().NotBeEmpty();
        response.Body.InvoiceValue.Should().Be(request.InvoiceValue);
        response.Body.Category.Should().NotBeNull();
        response.Body.Period.Should().NotBeNull();
        response.Body.Description.Should().Be(request.Description);
        response.Body.InvoiceDate.Should().Be(request.InvoiceDate);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenInvoiceValueIsNegativeAndCategoryIsCredit()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(-200)
            .WithCategoryId(Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceValueIsNegativeAndCategoryIsCredit()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(-200)
            .WithCategoryId(Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Invoice value cannot be negative with Credit transaction type");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturn400_WhenInvoiceValueIsPositiveAndCategoryIsDebit()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceValueIsPositiveAndCategoryIsDebit()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Message.Should().Be("Invoice value cannot be positive with Debit transaction type");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenPartsValueDoNotSumInvoice()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 1")
                        .WithValue(199).Build())
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Body.Message.Should().Be("Parts sum value do not match transaction value");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceCategoryIsCreditAndPartValueIsNegative()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 1")
                        .WithCategoryId(Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447"))
                        .WithValue(201).Build())
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 2")
                        .WithCategoryId(Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447"))
                        .WithValue(-1).Build())
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Body.Message.Should().Be("Transaction part value cannot be negative with Credit transaction type");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnErrorMessage_WhenInvoiceCategoryIsDebitAndPartValueIsPositive()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(-200)
            .WithCategoryId(Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 1")
                        .WithCategoryId(Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1"))
                        .WithValue(-201).Build())
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 2")
                        .WithCategoryId(Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1"))
                        .WithValue(1).Build())
            .Build();

        var response = await PostAsync<ErrorDto, AddTransactionRequest>(AddCategoryUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Body.Message.Should().Be("Transaction part value cannot be positive with Debit transaction type");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTransactionParts_WhenTransactionIsAdded()
    {
        var request = AddTransactionRequestBuilder
            .New()
            .WithInvoiceValue(200)
            .WithCategoryId(Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"))
            .WithPeriodId(Guid.Parse("7f565395-7c03-4519-8ead-1b5685e9716d"))
            .WithDescription("123456")
            .WithInvoiceDate(new DateOnly(2023, 11, 15))
                        .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 1")
                        .WithValue(199).Build())
            .AddPart(AddTransactionPartRequestBuilder
                        .New
                        .WithDescription("Description 2")
                        .WithValue(1).Build())
            .Build();

        var response = await PostAsync<AddTransactionResponse, AddTransactionRequest>(AddCategoryUrl, request);

        response.Body.Parts.Should().HaveCount(2);
        response.Body.Parts.Should()
            .AllSatisfy(x => x.Id.Should().NotBeEmpty())
            .And.Contain(x => x.Description == "Description 1" && x.Value == 199)
            .And.Contain(x => x.Description == "Description 2" && x.Value == 1);
    }

    #endregion
}
