using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WebApi;
using WebApi.Context;
using WebApi.Model;
using Xunit;

namespace IntegrationTests.Config;


public class IntegrationTestsBase : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly CustomWebApplicationFactory<Program> _applicationFactory;
    private readonly IServiceScope _scope;
    private readonly FinancesContext _context;

    private readonly HttpClient _httpClient;

    public IntegrationTestsBase(CustomWebApplicationFactory<Program> factory)
    {
        _applicationFactory = factory;
        _httpClient = _applicationFactory.CreateClient();

        _scope = _applicationFactory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<FinancesContext>();

        EnsureDatabaseCreated(_context);
        PopulateDb();
    }

    protected StringContent StringContent(object obj)
        => new(JsonSerializer.Serialize(obj), Encoding.UTF8, MediaTypeNames.Application.Json);

    protected async Task<HttpResponseHelper<TResponse>> PostAsync<TResponse, KRequest>(string url, KRequest body)
    {
        var response = await _httpClient.PostAsync(url, StringContent(body));
        return await BuildResponse<TResponse>(response);
    }

    protected Task<HttpResponseHelper<TResponse>> GetAsync<TResponse>(string url)
    {
        return GetAsync<TResponse>(url, null);
    }

    protected async Task<HttpResponseHelper> DeleteAsync(string url, params string[] pathParams)
    {
        var response = await _httpClient.DeleteAsync(GetUrl(url, pathParams));
        return await BuildResponse(response);
    }

    protected async Task<HttpResponseHelper<TResponse>> GetAsync<TResponse>(string url, params string[] pathParams)
    {
        var response = await _httpClient.GetAsync(GetUrl(url, pathParams));
        return await BuildResponse<TResponse>(response);
    }

    protected async Task<HttpResponseHelper<TResponse>> PutAsync<TResponse, KRequest>(string url, KRequest body, params string[] pathParams)
    {
        var response = await _httpClient.PutAsync(GetUrl(url, pathParams), StringContent(body));
        return await BuildResponse<TResponse>(response);
    }

    private async Task<HttpResponseHelper<TResponse>> BuildResponse<TResponse>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        return new HttpResponseHelper<TResponse>
        {
            Body = !string.IsNullOrWhiteSpace(body) ? JsonSerializer.Deserialize<TResponse>(body, _serializerOptions) : default,
            Response = response
        };
    }

    private async Task<HttpResponseHelper> BuildResponse(HttpResponseMessage response)
    {
        return await BuildResponse<object>(response);
    }

    private static string GetUrl(string url, params string[] pathParams)
    {
        return pathParams == null ? url : string.Format(url, pathParams);
    }


    private static void EnsureDatabaseCreated(FinancesContext context)
    {
        context.Database.EnsureCreated();
    }

    private void PopulateDb()
    {
        _context.Categories.AddRange(ReadDataFileAsync<Category>("Categories.json"));
        _context.Periods.AddRange(ReadDataFileAsync<Period>("Periods.json"));
        _context.Transactions.AddRange(ReadDataFileAsync<Transaction>("Transactions.json"));
        _context.FinancialGoals.AddRange(ReadDataFileAsync<FinancialGoal>("FinancialGoals.json"));

        _context.SaveChanges();
    }

    private static IEnumerable<T> ReadDataFileAsync<T>(string filename)
        => JsonSerializer.Deserialize<IEnumerable<T>>(File.ReadAllText(Path.Combine(".", "Data", filename)));

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _context.Database.EnsureDeleted();
        _context.Dispose();
        _scope.Dispose();
    }

    protected class HttpResponseHelper<T> : HttpResponseHelper
    {
        public T Body { get; set; }
    }

    protected class HttpResponseHelper
    {
        public HttpResponseMessage Response { get; set; }
    }

    //private static void GenerateTransactions()
    //{
    //    var categories = new[]
    //            {
    //        Guid.Parse("592d7199-92ad-4514-8fab-189a9b102df1"),
    //            Guid.Parse("265d4d24-e4a3-4e2b-b304-2a957bf191e0"),
    //            Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"),
    //            Guid.Parse("631e2ccb-325a-4ab2-8410-e342b28aa6e2")
    //    };

    //    var r = new Random();
    //    var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");

    //    var t = new Faker<Transaction>()
    //        .RuleFor(x => x.CategoryId, f => f.PickRandom(categories))
    //        .RuleFor(x => x.PeriodId, Guid.Parse("f0028ad8-e768-44f1-8798-f97e7b6bd7bf"))
    //        .RuleFor(x => x.InvoiceValue, f => f.Random.Decimal(-1000, 1000))
    //        .RuleFor(x => x.Description, lorem.Sentence(5, 2))
    //        .RuleFor(x => x.InvoiceDate, f => f.Date.BetweenDateOnly(new DateOnly(2024, 2, 15), new DateOnly(2024, 3, 14)))
    //        .GenerateBetween(10, 100);
    //}

    //private static void GenerateFinancialGoals()
    //{
    //    var creditIds = new[] { Guid.Parse("592d7199-92ad-4514-8fab-189a9b102df1"), Guid.Parse("265d4d24-e4a3-4e2b-b304-2a957bf191e0"), Guid.Parse("9b2230a8-471d-4991-9d81-c9b7edcd8fdf"), Guid.Parse("38b3c63a-8d73-4269-88d7-e58e1407e576"), Guid.Parse("efe5837e-4d4d-423b-8e18-9f0c78b5e850"), Guid.Parse("a4293376-2f66-42f3-8110-d0fc39a87ac3"), Guid.Parse("384278ec-7ee5-4a8f-8a6d-dbb42b9c4640"), Guid.Parse("400fe11a-73a1-41aa-b1ed-e479b26d3447") };
    //    var debitIds = new[] { Guid.Parse("631e2ccb-325a-4ab2-8410-e342b28aa6e2"), Guid.Parse("e858b4b9-f43e-4fbe-b12f-18170e39d140"), Guid.Parse("79cd7f4e-5d07-419a-ab8f-271faf6a36a1") };

    //    var fg = new List<FinancialGoal>();

    //    foreach(var creditId in creditIds)
    //    {
    //        fg.Add(new Faker<FinancialGoal>()
    //            .RuleFor(x => x.Goal, f => f.Random.Decimal(1200, 6200))
    //            .RuleFor(x => x.CategoryId, (f) => creditId)
    //            .RuleFor(x => x.PeriodId, f => Guid.Parse("f0028ad8-e768-44f1-8798-f97e7b6bd7bf"))
    //            .Generate());
    //    }

    //    foreach (var debitId in debitIds)
    //    {
    //        fg.Add(new Faker<FinancialGoal>()
    //            .RuleFor(x => x.Goal, f => f.Random.Decimal(-1800, -5))
    //            .RuleFor(x => x.CategoryId, (f) => debitId)
    //            .RuleFor(x => x.PeriodId, f => Guid.Parse("f0028ad8-e768-44f1-8798-f97e7b6bd7bf"))
    //            .Generate());
    //    }



    //}

}