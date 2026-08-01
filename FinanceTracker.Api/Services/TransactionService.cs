using FinanceTracker.Api.Helpers;
using FinanceTracker.Api.Models.DTOs;
using FinanceTracker.Api.Models.DTOs.Transactions;
using FinanceTracker.Api.Models.Entities;
using FinanceTracker.Api.Repositories;

namespace FinanceTracker.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResult<TransactionDto>> GetFilteredAsync(Guid userId, TransactionFilterRequest filter)
    {
        var (items, totalCount) = await _transactionRepository.GetFilteredAsync(userId, filter);
        return new PagedResult<TransactionDto>(items.Select(ToDto).ToList(), filter.Page, filter.PageSize, totalCount);
    }

    public async Task<TransactionDto> CreateAsync(Guid userId, CreateTransactionRequest request)
    {
        var category = await GetAccessibleCategoryAsync(userId, request.CategoryId);

        var transaction = new Transaction
        {
            UserId = userId,
            CategoryId = category.Id,
            Description = request.Description,
            Amount = request.Amount,
            Type = request.Type,
            Date = request.Date,
            Notes = request.Notes,
        };

        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        transaction.Category = category;
        return ToDto(transaction);
    }

    public async Task<TransactionDto> UpdateAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request)
    {
        var transaction = await GetOwnedTransactionAsync(userId, transactionId);
        var category = await GetAccessibleCategoryAsync(userId, request.CategoryId);

        transaction.Description = request.Description;
        transaction.Amount = request.Amount;
        transaction.Type = request.Type;
        transaction.Date = request.Date;
        transaction.CategoryId = category.Id;
        transaction.Notes = request.Notes;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _transactionRepository.SaveChangesAsync();

        transaction.Category = category;
        return ToDto(transaction);
    }

    public async Task DeleteAsync(Guid userId, Guid transactionId)
    {
        var transaction = await GetOwnedTransactionAsync(userId, transactionId);

        _transactionRepository.Remove(transaction);
        await _transactionRepository.SaveChangesAsync();
    }

    public async Task<byte[]> ExportCsvAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var transactions = await _transactionRepository.GetForExportAsync(userId, from, to);
        return CsvExporter.ExportTransactions(transactions);
    }

    private async Task<Transaction> GetOwnedTransactionAsync(Guid userId, Guid transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId)
            ?? throw new KeyNotFoundException("Transaction not found.");

        if (transaction.UserId != userId)
            throw new UnauthorizedAccessException("You do not own this transaction.");

        return transaction;
    }

    private async Task<Category> GetAccessibleCategoryAsync(Guid userId, Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId)
            ?? throw new KeyNotFoundException("Category not found.");

        if (category.UserId is not null && category.UserId != userId)
            throw new UnauthorizedAccessException("You do not have access to this category.");

        return category;
    }

    private static TransactionDto ToDto(Transaction t) => new(
        t.Id, t.Description, t.Amount, t.Type, t.Date, t.Notes,
        t.CategoryId, t.Category.Name, t.Category.Color, t.Category.Icon);
}
