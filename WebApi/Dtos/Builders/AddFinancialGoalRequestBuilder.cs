namespace WebApi.Dtos.Builders;

public class AddFinancialGoalRequestBuilder
{
    private readonly AddFinancialGoalRequest _financialGoal = new();

    public static AddFinancialGoalRequestBuilder New => new();
            
    public AddFinancialGoalRequestBuilder WithCategoryId(Guid categoryId)
    {
        _financialGoal.CategoryId = categoryId;
        return this;
    }
    public AddFinancialGoalRequestBuilder WithPeriodId(Guid periodId)
    {
        _financialGoal.PeriodId = periodId;
        return this;
    }
    public AddFinancialGoalRequestBuilder WithGoal(decimal goal)
    {
        _financialGoal.Goal = goal;
        return this;
    }
    public AddFinancialGoalRequest Build()
    {
        return _financialGoal;
    }
}
