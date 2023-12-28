using WebApi.Dtos;

namespace IntegrationTests.Builders;

public class EditPeriodsRequestBuilder
{
    private readonly EditPeriodRequest _periodRequest = new();

    public static EditPeriodsRequestBuilder New()
    {
        return new();
    }

    public EditPeriodsRequestBuilder WithStart(DateOnly? start) 
    {
        _periodRequest.Start = start;
        return this;
    }
    public EditPeriodsRequestBuilder WithEnd(DateOnly? end) 
    {
        _periodRequest.End = end;
        return this;
    }
    public EditPeriodsRequestBuilder WithName(string name) 
    {
        _periodRequest.Name = name;
        return this;
    }

    public EditPeriodRequest Build()
    {
        return _periodRequest;
    }
}
