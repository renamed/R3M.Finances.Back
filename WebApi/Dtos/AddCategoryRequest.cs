using System.ComponentModel.DataAnnotations;
using WebApi.Model;

namespace WebApi.Dtos;

public class AddCategoryRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    [Required]
    [EnumDataType(typeof(TransactionType))]
    public TransactionType TransactionType { get; set; }
    
    public Guid? ParentId { get; set; }
}
