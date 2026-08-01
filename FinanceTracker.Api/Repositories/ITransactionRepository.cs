using FinanceTracker.Api.Models.DTOs.Transactions;
using FinanceTracker.Api.Models.Entities;

namespace FinanceTracker.Api.Repositories;

public interface ITransactionRepository
{
    Task<(List<Transaction> Items, int TotalCount)> GetFilteredAsync(Guid userId, TransactionFilterRequest filter);
    Task<List<Transaction>> GetForExportAsync(Guid userId, DateOnly? from, DateOnly? to);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<List<Transaction>> GetForDashboardAsync(Guid userId, DateOnly from, DateOnly to);
    Task<List<Transaction>> GetAllForUserAsync(Guid userId);
    Task AddAsync(Transaction transaction);
    void Remove(Transaction transaction);
    Task SaveChangesAsync();
}
