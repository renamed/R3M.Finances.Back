namespace WebApi.Model.Builders;

public class CategoryBuilder
{
    private Category _category = new();

    public static CategoryBuilder New => new();

    public CategoryBuilder WithName(string name)
    {
        _category.Name = name;
        return this;
    }
    public CategoryBuilder WithId(Guid id)
    {
        _category.Id = id;
        return this;
    }
    public CategoryBuilder WithTransactionType(TransactionType transactionType)
    {
        _category.TransactionType = transactionType;
        return this;
    }
    public CategoryBuilder WithIsEssential(bool isEssential) 
    {
        _category.IsEssential = isEssential;
        return this;
    }
    public CategoryBuilder WithParentId(Guid? parentId)
    {
        _category.ParentId = parentId;
        return this;
    }
    public CategoryBuilder WithParent(Category parent)
    {
        _category.Parent = parent;
        return this;
    }
    public Category Build()
    {
        return _category;
    }

}
