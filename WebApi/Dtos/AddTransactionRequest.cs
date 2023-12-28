using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class AddTransactionRequest
{
    public DateOnly InvoiceDate { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 5)]
    public string Description { get; set; }
        
    public decimal InvoiceValue { get; set; }

    public Guid CategoryId { get; set; }
    public Guid PeriodId { get; set; }
}
