namespace WebApi.Model;

public class FinancialGoal : Register
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public Guid PeriodId { get; set; }
    public Period Period { get; set; }
    public decimal Goal { get; set; }
}
