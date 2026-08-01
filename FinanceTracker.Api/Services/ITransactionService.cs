using FinanceTracker.Api.Models.DTOs;
using FinanceTracker.Api.Models.DTOs.Transactions;

namespace FinanceTracker.Api.Services;

public interface ITransactionService
{
    Task<PagedResult<TransactionDto>> GetFilteredAsync(Guid userId, TransactionFilterRequest filter);
    Task<TransactionDto> CreateAsync(Guid userId, CreateTransactionRequest request);
    Task<TransactionDto> UpdateAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request);
    Task DeleteAsync(Guid userId, Guid transactionId);
    Task<byte[]> ExportCsvAsync(Guid userId, DateOnly? from, DateOnly? to);
}
