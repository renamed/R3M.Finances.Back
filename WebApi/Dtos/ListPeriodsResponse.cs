namespace WebApi.Dtos;

public class ListPeriodsResponse
{
    public Guid Id { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Name { get; set; }
    public bool IsCurrent { get; set; }
}
