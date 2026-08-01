using FinanceTracker.Api.Models.DTOs.Dashboard;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;

namespace FinanceTracker.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly ITransactionRepository _transactionRepository;

    public DashboardService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(Guid userId)
    {
        var transactions = await _transactionRepository.GetAllForUserAsync(userId);

        var balance = SignedTotal(transactions);

        var now = DateTime.UtcNow;
        var monthTransactions = transactions.Where(t => t.Date.Year == now.Year && t.Date.Month == now.Month);
        var monthlyIncome = monthTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var monthlyExpense = monthTransactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        return new DashboardSummaryDto(balance, monthlyIncome, monthlyExpense);
    }

    public async Task<List<CategoryBreakdownDto>> GetByCategoryAsync(Guid userId, int month, int year)
    {
        var from = new DateOnly(year, month, 1);
        var to = from.AddMonths(1).AddDays(-1);

        var transactions = await _transactionRepository.GetForDashboardAsync(userId, from, to);

        return transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.Category)
            .Select(g => new CategoryBreakdownDto(g.Key.Id, g.Key.Name, g.Key.Color, g.Sum(t => t.Amount)))
            .OrderByDescending(c => c.Total)
            .ToList();
    }

    public async Task<List<BalanceHistoryPointDto>> GetBalanceHistoryAsync(Guid userId, int months)
    {
        var transactions = await _transactionRepository.GetAllForUserAsync(userId);
        var today = DateTime.UtcNow;
        var startMonth = new DateOnly(today.Year, today.Month, 1).AddMonths(-(months - 1));

        var runningBalance = SignedTotal(transactions.Where(t => t.Date < startMonth));

        var result = new List<BalanceHistoryPointDto>();
        for (var i = 0; i < months; i++)
        {
            var monthStart = startMonth.AddMonths(i);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var monthTransactions = transactions.Where(t => t.Date >= monthStart && t.Date <= monthEnd).ToList();
            var income = monthTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = monthTransactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            runningBalance += income - expense;

            result.Add(new BalanceHistoryPointDto(monthStart.Year, monthStart.Month, income, expense, runningBalance));
        }

        return result;
    }

    private static decimal SignedTotal(IEnumerable<Transaction> transactions) =>
        transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
}
