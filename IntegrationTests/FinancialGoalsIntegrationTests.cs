using FluentAssertions;
using IntegrationTests.Config;
using System.Net;
using WebApi;
using WebApi.Dtos;
using WebApi.Dtos.Builders;
using Xunit;

namespace IntegrationTests;

public class FinancialGoalsIntegrationTests(CustomWebApplicationFactory<Program> applicationFactory) : IntegrationTestsBase(applicationFactory)
{
    
    private const string ListFinancialGoalsUrl = "api/financialgoals/period/";
    private const string GetFinancialGoalsUrl = "api/financialgoals/";
    private const string AddFinancialGoalsUrl = "api/financialgoals";
    private const string EditFinancialGoalsUrl = "api/financialgoals/";
    private const string DeleteFinancialGoalsUrl = "api/financialgoals/";


    [Fact]
    public async Task ListAsync_ShouldReturnAllRecords()
    {
        var url = ListFinancialGoalsUrl + "f0028ad8-e768-44f1-8798-f97e7b6bd7bf";
        var response = await GetAsync<IEnumerable<ListFinancialGoalResponse>>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Body.Should().HaveCount(11);
        response.Body.Should().AllSatisfy(x =>
            {
                x.Id.Should().NotBeEmpty();
                x.Category.Should().NotBeNull();
                x.Period.Should().NotBeNull();
                x.Goal.Should().NotBe(0);
            });
    }

    [Fact]
    public async Task GetAsync_ShouldReturnRecord_WhenIdExists()
    {
        var url = GetFinancialGoalsUrl + "86a85041-fe1c-4dd4-b031-fd60096f0113";
        var response = await GetAsync<ListFinancialGoalResponse>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldReturn404_WhenIdNotExists()
    {
        var url = GetFinancialGoalsUrl + "265d4d24-e4a3-4e2b-b304-2a957bf191e1";
        var response = await GetAsync<ErrorDto>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);        
    }

    [Fact]
    public async Task AddAsync_ShouldReturnRecord_WhenInserted()
    {
        var addFinancialGoalRequest = AddFinancialGoalRequestBuilder
                                        .New
                                        .WithCategoryId(Guid.Parse("265d4d24-e4a3-4e2b-b304-2a957bf191e0"))
                                        .WithPeriodId(Guid.Parse("2c47651d-b765-4e1a-8409-9a78bfc3e22c"))
                                        .WithGoal(666)
                                        .Build();

        var response = await PostAsync<AddFinancialGoalResponse, AddFinancialGoalRequest>(AddFinancialGoalsUrl, addFinancialGoalRequest);

        response.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        response.Body.Id.Should().NotBeEmpty();
        response.Body.Category.Should().NotBeNull();
        response.Body.Period.Should().NotBeNull();
        response.Body.Goal.Should().NotBe(0);
    }

    [Fact]
    public async Task EditAsync_ShouldReturnRecord_WhenInserted()
    {
        var editFinancialGoalRequest = EditFinancialGoalRequestBuilder
                                        .New
                                        .WithGoal(666)
                                        .Build();

        var putUrl = EditFinancialGoalsUrl + "5994529c-c085-4495-aecf-70729f7a5b4f";
        var getUrl = GetFinancialGoalsUrl  + "5994529c-c085-4495-aecf-70729f7a5b4f";
        var putResponse = await PutAsync<EditFinancialGoalResponse, EditFinancialGoalRequest>(putUrl, editFinancialGoalRequest);
        var getResponse = await GetAsync<ListFinancialGoalResponse>(getUrl);

        putResponse.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        putResponse.Body.Goal.Should().Be(666);
        putResponse.Body.Id.Should().Be(Guid.Parse("5994529c-c085-4495-aecf-70729f7a5b4f"));

        getResponse.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Body.Goal.Should().Be(666);
        getResponse.Body.Id.Should().Be(Guid.Parse("5994529c-c085-4495-aecf-70729f7a5b4f"));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete()
    {
        var deleteUrl = DeleteFinancialGoalsUrl + "86a85041-fe1c-4dd4-b031-fd60096f0113";
        var getUrl = GetFinancialGoalsUrl + "86a85041-fe1c-4dd4-b031-fd60096f0113";

        var deleteResponse = await DeleteAsync(deleteUrl);
        var getResponse = await GetAsync<ListFinancialGoalResponse>(getUrl);

        deleteResponse.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
