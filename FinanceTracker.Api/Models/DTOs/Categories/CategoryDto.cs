namespace FinanceTracker.Api.Models.DTOs.Categories;

public record CategoryDto(Guid Id, string Name, string Color, string Icon, bool IsGlobal);

public record CreateCategoryRequest(string Name, string Color, string Icon);

public record UpdateCategoryRequest(string Name, string Color, string Icon);
