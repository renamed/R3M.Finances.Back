using System.ComponentModel.DataAnnotations;
using WebApi.Model;

namespace WebApi.Dtos;

public class EditCategoryRequest
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
 
    [EnumDataType(typeof(TransactionType))]
    public TransactionType? TransactionType { get; set; }
}
