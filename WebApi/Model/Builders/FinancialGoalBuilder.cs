namespace WebApi.Model.Builders
{
    public class FinancialGoalBuilder
    {
        private readonly FinancialGoal _financialGoal = new();

        public static FinancialGoalBuilder New => new();

        public FinancialGoalBuilder WithId(Guid id)
        {
            _financialGoal.Id = id;
            return this;
        }
        public FinancialGoalBuilder WithCategoryId(Guid categoryId)
        {
            _financialGoal.CategoryId = categoryId;
            return this;
        }
        public FinancialGoalBuilder WithCategory(Category category)
        {
            _financialGoal.Category = category;
            return this;
        }
        public FinancialGoalBuilder WithPeriodId(Guid periodId)
        {
            _financialGoal.PeriodId = periodId;
            return this;
        }
        public FinancialGoalBuilder WithPeriod(Period period)
        {
            _financialGoal.Period = period;
            return this;
        }
        public FinancialGoalBuilder WithGoal(decimal goal)
        {
            _financialGoal.Goal = goal;
            return this;
        }
        public FinancialGoal Build()
        {
            return _financialGoal;
        }
    }
}
