namespace WebApi.Model;

public class Period : Register
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Name { get; set; }
}
