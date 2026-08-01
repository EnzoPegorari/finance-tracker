using FinanceTracker.Api.Models.DTOs.Categories;

namespace FinanceTracker.Api.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetForUserAsync(Guid userId);
    Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request);
    Task<CategoryDto> UpdateAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request);
    Task DeleteAsync(Guid userId, Guid categoryId);
}
