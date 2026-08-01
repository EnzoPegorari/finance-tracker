using FinanceTracker.Api.Data;
using FinanceTracker.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> GetForUserAsync(Guid userId) =>
        _context.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public Task<Category?> GetByIdAsync(Guid id) =>
        _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Category category) =>
        await _context.Categories.AddAsync(category);

    public void Remove(Category category) =>
        _context.Categories.Remove(category);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
