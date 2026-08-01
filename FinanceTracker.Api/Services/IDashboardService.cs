using FinanceTracker.Api.Models.DTOs.Dashboard;

namespace FinanceTracker.Api.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync(Guid userId);
    Task<List<CategoryBreakdownDto>> GetByCategoryAsync(Guid userId, int month, int year);
    Task<List<BalanceHistoryPointDto>> GetBalanceHistoryAsync(Guid userId, int months);
}
