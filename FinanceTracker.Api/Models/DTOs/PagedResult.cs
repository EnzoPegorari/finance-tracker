namespace FinanceTracker.Api.Models.DTOs;

public record PagedResult<T>(List<T> Items, int Page, int PageSize, int TotalCount);
