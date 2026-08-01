namespace FinanceTracker.Api.Models.DTOs.Dashboard;

public record DashboardSummaryDto(decimal Balance, decimal MonthlyIncome, decimal MonthlyExpense);

public record CategoryBreakdownDto(Guid CategoryId, string CategoryName, string Color, decimal Total);

public record BalanceHistoryPointDto(int Year, int Month, decimal Income, decimal Expense, decimal Balance);
