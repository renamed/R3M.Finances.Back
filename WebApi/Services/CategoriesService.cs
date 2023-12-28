using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Context;
using WebApi.Exceptions;
using WebApi.Extensions;
using WebApi.Model;

namespace WebApi.Services;

public class CategoriesService : ICategoriesService
{
    private readonly FinancesContext _financesContext;
    private readonly IMapper _mapper;

    public CategoriesService(IMapper mapper, FinancesContext financesContext)
    {
        _mapper = mapper;
        _financesContext = financesContext;
    }

    public Task<List<Category>> ListAsync()
    {
        return _financesContext.Categories.ToListAsync();
    }

    public Task<Category> GetAsync(Guid id)
    {
        return GetAsync(new Category { Id = id });
    }

    public Task<Category> GetAsync(Category category)
    {

        Expression<Func<Category, bool>> predicate = c => 1 == 1;

        predicate = predicate.AndIfNotDefault(x => x.Id == category.Id, category.Id);
        predicate = predicate.AndIfNotDefault(x => x.ParentId == category.ParentId, category.ParentId);
        predicate = predicate.AndIfNotDefault(x => x.Name == category.Name, category.Name);
        predicate = predicate.AndIf(x => x.TransactionType == category.TransactionType, category.TransactionType != TransactionType.Unknown);
        
        return _financesContext.Categories
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<Category> AddAsync(Category category)
    {
        if (category.ParentId.HasValue)
        {
            var parent = await GetAsync(category.ParentId.Value);
            if (parent == null)
            {
                throw new RecordNotFoundException("ParendId not found");
            }

            if (parent.TransactionType != category.TransactionType)
            {
                throw new ValidationException("Category must have the same transaction type of its parent");
            }
        }

        var byName = await GetAsync(new Category { Name = category.Name, TransactionType = category.TransactionType });
        if (byName != null)
        {
            throw new ValidationException("Category already exists");
        }

        
        await _financesContext.Categories.AddAsync(category);
        await _financesContext.SaveChangesAsync();

        return category;
    }

    public async Task<Category> EditAsync(Guid id, Category category)
    {
        var existingCategory = await GetAsync(id) 
            ?? throw new RecordNotFoundException("Category not found");

        if (category.Name == null && category.TransactionType == TransactionType.Unknown)
        {
            return existingCategory;
        }

        if (category.Name != null && 
            await GetAsync(new Category { Name = category.Name, TransactionType = category.TransactionType }) != null)
        {
            throw new ValidationException("Category already exists");
        }

        if (existingCategory.Parent != null 
            && category.TransactionType != existingCategory.Parent.TransactionType)
        {
            throw new ValidationException("Category must have the same transaction type of its parent");
        }

        _mapper.Map(category, existingCategory);
        await _financesContext.SaveChangesAsync();

        return existingCategory;
    }

    public async Task RemoveAsync(Guid id)
    {
        var category = await GetAsync(id) 
            ?? throw new RecordNotFoundException("Category not found");

        var isParent = await GetAsync(new Category { ParentId = id });
        if (isParent != null)
        {
            throw new ValidationException("Category has children");
        }

        _financesContext.Categories.Remove(category);
        await _financesContext.SaveChangesAsync();
    }
}
