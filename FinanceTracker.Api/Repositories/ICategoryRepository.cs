using FinanceTracker.Api.Models.Entities;

namespace FinanceTracker.Api.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetForUserAsync(Guid userId);
    Task<Category?> GetByIdAsync(Guid id);
    Task AddAsync(Category category);
    void Remove(Category category);
    Task SaveChangesAsync();
}
