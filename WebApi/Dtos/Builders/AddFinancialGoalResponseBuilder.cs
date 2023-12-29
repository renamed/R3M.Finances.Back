namespace WebApi.Dtos.Builders;

public class AddFinancialGoalResponseBuilder
{
    private readonly AddFinancialGoalResponse _financialGoal = new();

    public AddFinancialGoalResponseBuilder New => new();

    public AddFinancialGoalResponseBuilder WithId(Guid id)
    {
        _financialGoal.Id = id;
        return this;
    }
    public AddFinancialGoalResponseBuilder WithCategory(DefaultCategoryResponse defaultCategoryResponse)
    {
        _financialGoal.Category = defaultCategoryResponse;
        return this;
    }
    public AddFinancialGoalResponseBuilder WithPeriod(DefaultPeriodResponse defaultPeriodResponse)
    {
        _financialGoal.Period = defaultPeriodResponse;
        return this;
    }
    public AddFinancialGoalResponseBuilder WithGoal(decimal goal)
    {
        _financialGoal.Goal = goal;
        return this;
    }
    public AddFinancialGoalResponse Build()
    {
        return _financialGoal;
    }
}
