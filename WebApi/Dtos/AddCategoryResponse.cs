using System.Text.Json.Serialization;
using WebApi.Model;

namespace WebApi.Dtos;

public class AddCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType TransactionType { get; set; }
    public AddCategoryResponse Parent { get; set; }
}
