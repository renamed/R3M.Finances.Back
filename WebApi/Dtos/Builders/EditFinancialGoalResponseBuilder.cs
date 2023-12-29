namespace WebApi.Dtos.Builders;

public class EditFinancialGoalResponseBuilder
{
    private readonly EditFinancialGoalResponse _financialGoal = new();

    public AddFinancialGoalResponseBuilder New => new();

    public EditFinancialGoalResponseBuilder WithId(Guid id)
    {
        _financialGoal.Id = id;
        return this;
    }
    public EditFinancialGoalResponseBuilder WithCategory(DefaultCategoryResponse defaultCategoryResponse)
    {
        _financialGoal.Category = defaultCategoryResponse;
        return this;
    }
    public EditFinancialGoalResponseBuilder WithPeriod(DefaultPeriodResponse defaultPeriodResponse)
    {
        _financialGoal.Period = defaultPeriodResponse;
        return this;
    }
    public EditFinancialGoalResponseBuilder WithGoal(decimal goal)
    {
        _financialGoal.Goal = goal;
        return this;
    }
    public EditFinancialGoalResponse Build()
    {
        return _financialGoal;
    }
}
