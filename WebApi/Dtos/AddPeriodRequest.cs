using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class AddPeriodRequest
{
    [Required]
    public DateOnly Start { get; set; }
    [Required]
    public DateOnly End { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 6)]
    public string Name { get; set; }
}
