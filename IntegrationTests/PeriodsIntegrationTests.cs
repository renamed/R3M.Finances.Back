using FluentAssertions;
using IntegrationTests.Builders;
using IntegrationTests.Config;
using System.Net;
using WebApi;
using WebApi.Dtos;
using Xunit;

namespace IntegrationTests;

public class PeriodsIntegrationTests(CustomWebApplicationFactory<Program> applicationFactory) : IntegrationTestsBase(applicationFactory)
{
    private const string AddPeriodsUrl = "api/periods";
    private const string EditPeriodsUrl = "api/periods/";
    private const string GetPeriodsUrl = "api/periods/";
    private const string ListPeriodsUrl = "api/periods";
    private const string GetPeriodsByDateUrl = "api/periods/date/";
    private const string DeletePeriodsUrl = "api/periods/";

    #region GET        

    [Fact]
    public async Task ListAsync_ShouldReturn200_WhenCalled()
    {
        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(ListPeriodsUrl);
        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListAsync_ShouldListAllElements_WhenCalled()
    {
        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(ListPeriodsUrl);
        response.Body.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOk_WhenEndDateIsNotPassed()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldList_WhenEndDateIsNotPassed()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        var body = response.Body;
        body.Should().HaveCount(2);

        AssertPeriods(body.ElementAt(0), new ListPeriodsResponse
        {
            Id = Guid.Parse("c1d90b55-9195-4841-8209-be1c85eae464"),
            Start = new DateOnly(2024, 1, 15),
            End = new DateOnly(2024, 2, 14),
            Name = "202401"
        });

        AssertPeriods(body.ElementAt(1), new ListPeriodsResponse
        {
            Id = Guid.Parse("f0028ad8-e768-44f1-8798-f97e7b6bd7bf"),
            Start = new DateOnly(2024, 2, 15),
            End = new DateOnly(2024, 3, 14),
            Name = "202402"
        });
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOk_WhenEndDateIsPassed()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15?end=2024-02-14";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldList_WhenEndDateIsPassed()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15?end=2024-02-14";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        var body = response.Body;
        body.Should().HaveCount(1);

        AssertPeriods(body.ElementAt(0), new ListPeriodsResponse
        {
            Id = Guid.Parse("c1d90b55-9195-4841-8209-be1c85eae464"),
            Start = new DateOnly(2024, 1, 15),
            End = new DateOnly(2024, 2, 14),
            Name = "202401"
        });
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOk_WhenNoElementsAreReturned()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15?end=2024-01-16";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldReturn404_WhenIdDoesNotExist()
    {
        var url = GetPeriodsUrl + Guid.NewGuid();

        var response = await GetAsync<ErrorDto>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnErrorMessage_WhenIdDoesNotExist()
    {
        var url = GetPeriodsUrl + Guid.NewGuid();

        var response = await GetAsync<ErrorDto>(url);

        response.Body.Message.Should().Be("Period not found");
    }

    [Fact]
    public async Task GetAsync_ShouldReturn404_WhenIdNotFound()
    {
        var url = GetPeriodsByDateUrl + "2024-01-15?end=2024-01-16";

        var response = await GetAsync<IEnumerable<ListPeriodsResponse>>(url);

        var body = response.Body;
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAsync_ShouldReturn200_WhenIdFound()
    {
        var url = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var response = await GetAsync<ListPeriodsResponse>(url);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnObject_WhenIdFound()
    {
        var url = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var response = await GetAsync<ListPeriodsResponse>(url);

        AssertPeriods(new ListPeriodsResponse
        {
            Id = Guid.Parse("2c47651d-b765-4e1a-8409-9a78bfc3e22c"),
            Start = DateOnly.Parse("2023-12-01"),
            End = DateOnly.Parse("2024-01-14"),
            Name = "202312",
        }, response.Body);
    }

    private static void AssertPeriods(ListPeriodsResponse target, ListPeriodsResponse response)
    {
        target.Id.Should().Be(response.Id);
        target.Start.Should().Be(response.Start);
        target.End.Should().Be(response.End);
        target.Name.Should().Be(response.Name);
    }

    #endregion

    #region POST        

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenStartIsNull()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("Random Name")
            .WithEnd(DateOnly.FromDateTime(DateTime.Now))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenEndIsNull()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("Random Name")
            .WithStart(DateOnly.FromDateTime(DateTime.Now))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameIsNull()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithStart(DateOnly.FromDateTime(DateTime.Now))
            .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameLengthIsLessThen6()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("12345")
            .WithStart(DateOnly.FromDateTime(DateTime.Now))
            .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameLengthIsGreaterThen10()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("12345678901")
            .WithStart(DateOnly.FromDateTime(DateTime.Now))
            .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenStartIsAfterEnd()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("12345678901")
            .WithEnd(DateOnly.FromDateTime(DateTime.Now))
            .WithStart(DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnErrorMessage_WhenStartIsAfterEnd()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("1234567")
            .WithEnd(DateOnly.FromDateTime(DateTime.Now))
            .WithStart(DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Body.Message.Should().Be("End date before Start");
    }

    [Theory]
    [MemberData(nameof(GetOverlappingStartEndDates))]
    public async Task AddAsync_ShouldReturn400_WhenOverlappingStartEndDates(DateOnly start, DateOnly end)
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202301")
            .WithStart(start)
            .WithEnd(end)
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(GetOverlappingStartEndDates))]
    public async Task AddAsync_ShouldEnsureErrorMessage_WhenOverlappingStartEndDates(DateOnly start, DateOnly end)
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202301")
            .WithStart(start)
            .WithEnd(end)
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Body.Message.Should().Be("Overlapping Period already exists");
    }

    [Fact]
    public async Task AddAsync_ShouldReturn400_WhenNameAlreadyExists()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202401")
            .WithStart(new DateOnly(2018, 1, 1))
            .WithEnd(new DateOnly(2018, 2, 1))
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnErrorMessage_WhenNameAlreadyExists()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202401")
            .WithStart(new DateOnly(2018, 1, 1))
            .WithEnd(new DateOnly(2018, 2, 1))
            .Build();

        var response = await PostAsync<ErrorDto, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Body.Message.Should().Be("Name already exists");
    }

    [Fact]
    public async Task AddAsync_ShouldReturn201_WhenInserted()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("201906")
            .WithStart(new DateOnly(2019, 6, 1))
            .WithEnd(new DateOnly(2019, 7, 1))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnNewPeriod_WhenInserted()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202002")
            .WithStart(new DateOnly(2020, 2, 2))
            .WithEnd(new DateOnly(2020, 3, 1))
            .Build();

        var response = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);

        response.Body.Id.Should().NotBeEmpty();
        AssertPeriods(response.Body, new AddPeriodResponse
        {
            Name = request.Name,
            Start = request.Start,
            End = request.End
        });
    }

    [Fact]
    public async Task AddAsync_ShouldBeAvailable_WhenInserted()
    {
        var request = AddPeriodsRequestBuilder
            .New()
            .WithName("202003")
            .WithStart(new DateOnly(2020, 3, 2))
            .WithEnd(new DateOnly(2020, 3, 4))
            .Build();

        var addResponse = await PostAsync<AddPeriodResponse, AddPeriodRequest>(AddPeriodsUrl, request);
        var getResponse = await GetAsync<ListPeriodsResponse>(GetPeriodsUrl + addResponse.Body.Id);

        AssertPeriods(addResponse.Body, getResponse.Body);
    }
    #endregion

    #region PUT

    [Fact]
    public async Task PutAsync_ShouldNotUpdateStart_WhenStartIsNull()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("Random Name")
            .WithStart(null)
            .WithEnd(new DateOnly(1999,2,2))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Start.Should().Be(getOneResponse.Body.Start);
    }

    [Fact]
    public async Task PutAsync_ShouldNotUpdateEnd_WhenEndIsNull()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("Random Name")
            .WithStart(new DateOnly(1999, 2, 1))
            .WithEnd(null)                
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutAsync_ShouldNotUpdateName_WhenNameIsNull()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName(null)
            .WithStart(new DateOnly(1999, 3, 1))
            .WithEnd(new DateOnly(1999, 3, 2))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturn400_WhenNameLengthIsLessThen6()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("1234")
            .WithStart(new DateOnly(1999, 4, 1))
            .WithEnd(new DateOnly(1999, 4, 2))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturn400_WhenNameLengthIsGreaterThen10()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("12344567890")
            .WithStart(new DateOnly(1999, 4, 1))
            .WithEnd(new DateOnly(1999, 4, 2))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturn400_WhenStartIsAfterEnd()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("123456")
            .WithStart(new DateOnly(1999, 5, 2))
            .WithEnd(new DateOnly(1999, 5, 1))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturnErrorMessage_WhenStartIsAfterEnd()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("1234567")
            .WithStart(new DateOnly(1999, 5, 5))
            .WithEnd(new DateOnly(1999, 5, 3))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        response.Body.Message.Should().Be("End date before Start");
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Theory]
    [MemberData(nameof(GetOverlappingStartEndDates))]
    public async Task PutAsync_ShouldReturn400_WhenOverlappingStartEndDates(DateOnly start, DateOnly end)
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("202301")
            .WithStart(start)
            .WithEnd(end)
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        response.Body.Message.Should().Be("Overlapping Period already exists");
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturn400_WhenNameAlreadyExists()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("202402")
            .WithStart(new DateOnly(1999, 5, 1))
            .WithEnd(new DateOnly(1999, 5, 2))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<ErrorDto, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        getTwoResponse.Body.Name.Should().Be(getOneResponse.Body.Name);
    }

    [Fact]
    public async Task PutAsync_ShouldReturn200_WhenUpdated()
    {
        var request = EditPeriodsRequestBuilder
            .New()
            .WithName("199906")
            .WithStart(new DateOnly(2019, 6, 1))
            .WithEnd(new DateOnly(2019, 7, 1))
            .Build();

        var putUrl = EditPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";
        var getUrl = GetPeriodsUrl + "2c47651d-b765-4e1a-8409-9a78bfc3e22c";

        var getOneResponse = await GetAsync<ListPeriodsResponse>(getUrl);
        var response = await PutAsync<EditPeriodResponse, EditPeriodRequest>(putUrl, request);
        var getTwoResponse = await GetAsync<ListPeriodsResponse>(getUrl);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);

        getTwoResponse.Body.Id.Should().Be("2c47651d-b765-4e1a-8409-9a78bfc3e22c");
        getTwoResponse.Body.Name.Should().Be(request.Name);
        getTwoResponse.Body.Start.Should().Be(request.Start);
        getTwoResponse.Body.End.Should().Be(request.End);
                    
        getTwoResponse.Body.Name.Should().NotBe(getOneResponse.Body.Name);
        getTwoResponse.Body.Start.Should().NotBe(getOneResponse.Body.Start);
        getTwoResponse.Body.End.Should().NotBe(getOneResponse.Body.End);
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


    public static TheoryData<DateOnly, DateOnly> GetOverlappingStartEndDates
    {
        get => new()
        {
            { new DateOnly(2023, 12, 1), new DateOnly(2023, 12, 10) },
            { new DateOnly(2023, 11, 25), new DateOnly(2023, 12, 10) },
            { new DateOnly(2023, 12, 25), new DateOnly(2024, 1, 13) },
            { new DateOnly(2021, 12, 25), new DateOnly(2025, 1, 13) }
        };
    }

    private static void AssertPeriods(AddPeriodResponse target, AddPeriodResponse response)
    {
        target.Start.Should().Be(response.Start);
        target.End.Should().Be(response.End);
        target.Name.Should().Be(response.Name);
    }

    private static void AssertPeriods(AddPeriodResponse target, ListPeriodsResponse response)
    {
        target.Id.Should().Be(response.Id);
        target.Start.Should().Be(response.Start);
        target.End.Should().Be(response.End);
        target.Name.Should().Be(response.Name);
    }
}
