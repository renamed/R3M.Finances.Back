using System.Text.Json.Serialization;
using WebApi.Model;

namespace WebApi.Dtos;

public class EditCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType TransactionType { get; set; }
    public EditCategoryResponse Parent { get; set; }
    public bool IsEssential { get; set; }
}
