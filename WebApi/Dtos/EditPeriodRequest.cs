using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class EditPeriodRequest
{
    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }
    
    [StringLength(10, MinimumLength = 6)]
    public string Name { get; set; }
}
