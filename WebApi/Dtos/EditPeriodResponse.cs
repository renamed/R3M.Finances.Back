namespace WebApi.Dtos;

public class EditPeriodResponse
{
    public Guid Id { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Name { get; set; }
}
