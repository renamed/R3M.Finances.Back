namespace WebApi.Dtos.Builders;

public class EditFinancialGoalRequestBuilder
{
    private readonly EditFinancialGoalRequest _financialGoal = new();

    public static EditFinancialGoalRequestBuilder New => new();

    public EditFinancialGoalRequestBuilder WithGoal(decimal goal)
    {
        _financialGoal.Goal = goal;
        return this;
    }
    public EditFinancialGoalRequest Build()
    {
        return _financialGoal;
    }
}
