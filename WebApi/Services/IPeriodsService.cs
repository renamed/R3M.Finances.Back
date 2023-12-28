using WebApi.Model;

namespace WebApi.Services;

public interface IPeriodsService
{
    Task<Period> AddAsync(Period period);
    Task DeleteAsync(Guid id);
    Task<Period> EditAsync(Guid id, Period period);
    Task<List<Period>> GetAsync(Period period);
    Task<Period> GetOneAsync(Period period);
    Task<Period> GetOneAsync(Guid id);
    Task<Period> GetOneAsync(string name);
    Task<List<Period>> ListAsync();
}