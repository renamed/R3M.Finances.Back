using WebApi.Model;

namespace WebApi.Dtos;

public class AddFinancialGoalRequest
{
    public Guid CategoryId { get; set; }        
    public Guid PeriodId { get; set; }        
    public decimal Goal { get; set; }
}
