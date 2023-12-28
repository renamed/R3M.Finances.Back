using WebApi.Dtos;

namespace IntegrationTests.Builders;

public class AddPeriodsRequestBuilder
{
    private readonly AddPeriodRequest _periodRequest = new();

    public static AddPeriodsRequestBuilder New()
    {
        return new();
    }

    public AddPeriodsRequestBuilder WithStart(DateOnly start) 
    {
        _periodRequest.Start = start;
        return this;
    }
    public AddPeriodsRequestBuilder WithEnd(DateOnly end) 
    {
        _periodRequest.End = end;
        return this;
    }
    public AddPeriodsRequestBuilder WithName(string name) 
    {
        _periodRequest.Name = name;
        return this;
    }

    public AddPeriodRequest Build()
    {
        return _periodRequest;
    }
}
