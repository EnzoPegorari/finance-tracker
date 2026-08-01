using FinanceTracker.Api.Models.DTOs.Categories;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;
using FinanceTracker.Api.Services;
using Moq;
using Xunit;

namespace FinanceTracker.Api.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly CategoryService _sut;
    private readonly Guid _userId = Guid.NewGuid();

    public CategoryServiceTests()
    {
        _sut = new CategoryService(_categoryRepository.Object);
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryBelongsToAnotherUser_ThrowsUnauthorizedAccessException()
    {
        var category = new Category { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Other" };
        _categoryRepository.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var request = new UpdateCategoryRequest("New name", "#FFFFFF", "icon");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.UpdateAsync(_userId, category.Id, request));
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryIsGlobal_ThrowsUnauthorizedAccessException()
    {
        var globalCategory = new Category { Id = Guid.NewGuid(), UserId = null, Name = "Alimentação" };
        _categoryRepository.Setup(r => r.GetByIdAsync(globalCategory.Id)).ReturnsAsync(globalCategory);

        var request = new UpdateCategoryRequest("Hacked", "#FFFFFF", "icon");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.UpdateAsync(_userId, globalCategory.Id, request));
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryDoesNotExist_ThrowsKeyNotFoundException()
    {
        _categoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.DeleteAsync(_userId, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_SetsUserIdAndPersists()
    {
        var request = new CreateCategoryRequest("Pets", "#00FF00", "paw");

        var result = await _sut.CreateAsync(_userId, request);

        Assert.Equal("Pets", result.Name);
        Assert.False(result.IsGlobal);
        _categoryRepository.Verify(r => r.AddAsync(It.Is<Category>(c => c.UserId == _userId)), Times.Once);
    }
}
