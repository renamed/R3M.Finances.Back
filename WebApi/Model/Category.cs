namespace WebApi.Model;

public class Category : Register
{
    public string Name { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid? ParentId { get; set; }
    public Category Parent { get; set; }
    public bool IsEssential { get; set; }
}
