using FinanceTracker.Api.Models.DTOs.Categories;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;

namespace FinanceTracker.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetForUserAsync(Guid userId)
    {
        var categories = await _categoryRepository.GetForUserAsync(userId);
        return categories.Select(ToDto).ToList();
    }

    public async Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request)
    {
        var category = new Category
        {
            UserId = userId,
            Name = request.Name,
            Color = request.Color,
            Icon = request.Icon,
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return ToDto(category);
    }

    public async Task<CategoryDto> UpdateAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request)
    {
        var category = await GetOwnedCategoryAsync(userId, categoryId);

        category.Name = request.Name;
        category.Color = request.Color;
        category.Icon = request.Icon;

        await _categoryRepository.SaveChangesAsync();

        return ToDto(category);
    }

    public async Task DeleteAsync(Guid userId, Guid categoryId)
    {
        var category = await GetOwnedCategoryAsync(userId, categoryId);

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync();
    }

    private async Task<Category> GetOwnedCategoryAsync(Guid userId, Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId)
            ?? throw new KeyNotFoundException("Category not found.");

        if (category.UserId != userId)
            throw new UnauthorizedAccessException("You do not own this category.");

        return category;
    }

    private static CategoryDto ToDto(Category category) =>
        new(category.Id, category.Name, category.Color, category.Icon, category.UserId == null);
}
