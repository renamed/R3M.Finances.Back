namespace WebApi.Dtos;

public class EditFinancialGoalResponse
{
    public Guid Id { get; set; }
    public DefaultCategoryResponse Category { get; set; }
    public DefaultPeriodResponse Period { get; set; }
    public decimal Goal { get; set; }
}
