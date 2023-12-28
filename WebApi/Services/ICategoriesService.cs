using WebApi.Model;

namespace WebApi.Services;

public interface ICategoriesService
{
    Task<Category> AddAsync(Category category);
    Task<Category> EditAsync(Guid id, Category category);
    Task<Category> GetAsync(Guid id);
    Task<Category> GetAsync(Category category);
    Task<List<Category>> ListAsync();
    Task RemoveAsync(Guid id);
}