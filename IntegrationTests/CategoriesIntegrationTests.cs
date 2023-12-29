using FluentAssertions;
using IntegrationTests.Config;
using System.Net;
using WebApi;
using WebApi.Dtos;
using WebApi.Dtos.Builders;
using WebApi.Model;
using Xunit;

namespace IntegrationTests;

public class CategoriesIntegrationTests(CustomWebApplicationFactory<Program> applicationFactory) : IntegrationTestsBase(applicationFactory)
{
    
    private const string ListCategoryUrl = "api/categories";
    private const string GetCategoryUrl = "api/categories/{0}";
    private const string PostCategoryUrl = "api/categories";
    private const string PutCategoryUrl = "api/categories/{0}";
    private const string CreditExistingId = "265d4d24-e4a3-4e2b-b304-2a957bf191e0";
    private const string DebitExistingId = "e858b4b9-f43e-4fbe-b12f-18170e39d140";
    private const string NonExistingId = "265d4d24-e4a3-4e2b-b304-2a957bf191e1";
    private const string DeletePeriodsUrl = "api/periods/";
    private const string GetPeriodsUrl = "api/periods/";

    #region GET
    [Fact]
    public async Task ListCategories_ShouldReturn200_WhenCalled()
    {
        var response = await GetAsync<IEnumerable<ListCategoriesResponse>>(ListCategoryUrl);
        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListCategories_ShouldReturn3Parents_WhenCalled()
    {
        var response = await GetAsync<IEnumerable<ListCategoriesResponse>>(ListCategoryUrl);
        response.Body.Should().HaveCount(3);
    }

    [Fact]
    public async Task ListCategories_ShouldReturnParentList_WhenCalled()
    {
        var response = await GetAsync<IEnumerable<ListCategoriesResponse>>(ListCategoryUrl);

        response.Body.Should().ContainSingle(x => x.Name == "Parent Category 1")
            .And.ContainSingle(x => x.Name == "Parent Category 2")
            .And.ContainSingle(x => x.Name == "Parent Category 3");
    }

    [Fact]
    public async Task ListCategories_ShouldReturnChildren_WhenParentHasChildren()
    {
        var response = await GetAsync<IEnumerable<ListCategoriesResponse>>(ListCategoryUrl);
        var body = response.Body;

        var parent = body.First(x => x.Id == Guid.Parse("592d7199-92ad-4514-8fab-189a9b102df1"));

        HasChildren(parent.Children,
            "265d4d24-e4a3-4e2b-b304-2a957bf191e0",
            "9b2230a8-471d-4991-9d81-c9b7edcd8fdf",
            "38b3c63a-8d73-4269-88d7-e58e1407e576");

        HasChildren(parent.Children.First(x => x.Id == Guid.Parse("265d4d24-e4a3-4e2b-b304-2a957bf191e0")).Children,
            "efe5837e-4d4d-423b-8e18-9f0c78b5e850");

        HasChildren(body.First(x => x.Id == Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1")).Children, null);
    }

    //[Theory]
    //[InlineData("592d7199-92ad-4514-8fab-189a9b102df1", TransactionType.Credit)]
    //[InlineData("79cd7f4e-5d07-419a-ab8f-271faf6a36a1", TransactionType.Debit)]
    //public async Task ListCategories_ShouldMapTransactionType(string guid, TransactionType expected)
    //{
    //    var response = await GetAsync<IEnumerable<ListCategoriesResponse>>(ListCategoryUrl);

    //    response.Body.First(x => x.Id == Guid.Parse(guid)).TransactionType
    //        .Should().Be(expected);
    //}

    [Fact]
    public async Task GetAsync_ShouldReturn404_WhenIdNoExists()
    {
        var categoryId = Guid.NewGuid().ToString();

        var response = await GetAsync<ListCategoriesResponse>(GetCategoryUrl, categoryId);

        response.Response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAsync_ShouldReturn200_WhenIdExists()
    {
        var categoryId = "592d7199-92ad-4514-8fab-189a9b102df1";

        var response = await GetAsync<ListCategoriesResponse>(GetCategoryUrl, categoryId);

        response.Response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCategory_WhenIdExists()
    {
        var categoryId = "e858b4b9-f43e-4fbe-b12f-18170e39d140";

        var response = await GetAsync<ListCategoriesResponse>(GetCategoryUrl, categoryId);


        response.Body.Should().NotBeNull();
        response.Body.Name.Should().NotBeNull().And.Be("Child Category 2.1");
        response.Body.TransactionType.Should().Be(TransactionType.Debit);
        response.Body.Children.Should().BeNullOrEmpty();
        response.Body.Id.Should().Be(categoryId);
    }

    private static void HasChildren(List<ListCategoriesResponse> listCategories, params string[] childrenId)
    {
        listCategories
            .Should().NotBeNull();

        if (childrenId == null || childrenId.Length == 0)
        {
            listCategories.Should().BeNullOrEmpty();
        }
        else
        {
            listCategories.Should().HaveCount(childrenId.Length);
            foreach (var currGuid in childrenId)
            {
                listCategories.Should().ContainSingle(x => x.Id == Guid.Parse(currGuid));
            }
        }
    }
    #endregion

    #region POST
    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameIsNotProvided()
    {
        var request = AddCategoryRequestBuilder.New()
            .WithName(null)
            .WithTransactionType(TransactionType.Debit)
            .Build();

        var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameHasLessThan3Chars()
    {
        var request = AddCategoryRequestBuilder.New()
            .WithName("ab")
            .WithTransactionType(TransactionType.Debit)
            .Build();

        var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameHasMoreThan100Chars()
    {
        var request = AddCategoryRequestBuilder.New()
            .WithName(new string('a', 101))
            .WithTransactionType(TransactionType.Debit)
            .Build();

        var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    //[Fact]
    //public async Task AddAsync_ShouldReturn404_WhenParentIdDoesNotExist()
    //{
    //    var request = AddCategoryRequestBuilder.New()
    //        .WithName("Abcd")
    //        .WithTransactionType(TransactionType.Debit)
    //        .WithParentId(Guid.Parse("38b3c63a-8d73-4269-88d7-e58e1407e577"))
    //        .Build();

    //    var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
    //    response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    //}

    //[Fact]
    //public async Task AddAsync_ShouldReturn400_WhenNameAlreadyExists()
    //{
    //    var request = AddCategoryRequestBuilder.New()
    //        .WithName("Child Category 1.3")
    //        .WithTransactionType(TransactionType.Debit)
    //        .WithParentId(Guid.Parse("38b3c63a-8d73-4269-88d7-e58e1407e576"))
    //        .Build();

    //    var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
    //    response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //}

    [Fact]
    public async Task AddAsync_ShouldReturn201_WhenCategoryIsAdded()
    {
        var request = AddCategoryRequestBuilder.New()
            .WithName("My Brand New Category")
            .WithTransactionType(TransactionType.Credit)
            .WithParentId(Guid.Parse("38b3c63a-8d73-4269-88d7-e58e1407e576"))
            .Build();

        var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);
        response.Response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnBody_WhenCategoryIsAdded()
    {
        const string Name = "My Brand New Category";
        const TransactionType TransactionType = TransactionType.Credit;

        var request = AddCategoryRequestBuilder.New()
            .WithName(Name)
            .WithTransactionType(TransactionType)
            .WithParentId(Guid.Parse("38b3c63a-8d73-4269-88d7-e58e1407e576"))
            .Build();

        var response = await PostAsync<AddCategoryResponse, AddCategoryRequest>(PostCategoryUrl, request);

        response.Body.Id.Should().NotBeEmpty();
        response.Body.Name.Should().Be(Name);
        response.Body.TransactionType.Should().Be(TransactionType);
        response.Body.Parent.Should().NotBeNull();
    }

    #endregion

    #region PUT

    [Fact]
    public async Task EditAsync_ShouldNotUpdateName_WhenNameIsNotProvided()
    {
        var request = EditCategoryRequestBuilder.New()
            .WithName(null)
            .WithTransactionType(TransactionType.Credit)
            .Build();

        var putResponse = await PutAsync<EditCategoryResponse, EditCategoryRequest>(PutCategoryUrl, request, CreditExistingId);
        putResponse.Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await GetAsync<ListCategoriesResponse>(GetCategoryUrl, CreditExistingId);
        getResponse.Body.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task EditAsync_ShouldNotUpdateTransactionType_WhenNotProvided()
    {
        var request = EditCategoryRequestBuilder.New()
            .WithTransactionType(null)
            .Build();

        var putResponse = await PutAsync<EditCategoryResponse, EditCategoryRequest>(PutCategoryUrl, request, DebitExistingId);
        putResponse.Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await GetAsync<ListCategoriesResponse>(GetCategoryUrl, CreditExistingId);
        getResponse.Body.TransactionType.Should().Be(TransactionType.Credit);
    }

    [Fact]
    public async Task EditAsync_ShouldReturn400_WhenNameHasLessThan3Chars()
    {
        var request = EditCategoryRequestBuilder.New()
            .WithName("ab")
            .WithTransactionType(TransactionType.Debit)
            .Build();

        var response = await PutAsync<EditCategoryResponse, EditCategoryRequest>(PutCategoryUrl, request, CreditExistingId);
        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Editsync_ShouldReturn400_WhenNameHasMoreThan100Chars()
    {
        var request = EditCategoryRequestBuilder.New()
            .WithName(new string('a', 101))
            .WithTransactionType(TransactionType.Debit)
            .Build();

        var response = await PutAsync<EditCategoryResponse, EditCategoryRequest>(PutCategoryUrl, request, CreditExistingId);
        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task ShouldReturn404_WhenRegisterDoesNotExist()
    {
        var url = DeletePeriodsUrl + Guid.NewGuid();

        var response = await DeleteAsync(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldReturn200_WhenRegisterExists()
    {
        var url = DeletePeriodsUrl + "f0028ad8-e768-44f1-8798-f97e7b6bd7bf";

        var response = await DeleteAsync(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ShouldReturnCorrectCount_WhenRegisterExist()
    {
        var url = DeletePeriodsUrl + "c1d90b55-9195-4841-8209-be1c85eae464";

        var getOne = await GetAsync<IEnumerable<ListPeriodsResponse>>(GetPeriodsUrl);
        await DeleteAsync(url);
        var getTwo = await GetAsync<IEnumerable<ListPeriodsResponse>>(GetPeriodsUrl);

        getOne.Body.Should().HaveCount(getTwo.Body.Count() + 1);
    }

    #endregion
}
