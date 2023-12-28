using System.Text.Json.Serialization;
using WebApi.Model;

namespace WebApi.Dtos;

public class ListCategoriesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType TransactionType { get; set; }

    public List<ListCategoriesResponse> Children { get; set; } = new();
    public bool IsEssential { get; set; }
}
