using FinanceTracker.Api.Data;
using FinanceTracker.Api.Models.DTOs.Transactions;
using FinanceTracker.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Transaction> Items, int TotalCount)> GetFilteredAsync(Guid userId, TransactionFilterRequest filter)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        query = ApplyFilters(query, filter.From, filter.To, filter.CategoryId, filter.Type);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<List<Transaction>> GetForExportAsync(Guid userId, DateOnly? from, DateOnly? to)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        query = ApplyFilters(query, from, to, null, null);

        return query.OrderByDescending(t => t.Date).ToListAsync();
    }

    public Task<Transaction?> GetByIdAsync(Guid id) =>
        _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);

    public Task<List<Transaction>> GetForDashboardAsync(Guid userId, DateOnly from, DateOnly to) =>
        _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.Date >= from && t.Date <= to)
            .ToListAsync();

    public Task<List<Transaction>> GetAllForUserAsync(Guid userId) =>
        _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(Transaction transaction) =>
        await _context.Transactions.AddAsync(transaction);

    public void Remove(Transaction transaction) =>
        _context.Transactions.Remove(transaction);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Transaction> ApplyFilters(
        IQueryable<Transaction> query, DateOnly? from, DateOnly? to, Guid? categoryId, string? type)
    {
        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);
        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(t => t.Type == type);

        return query;
    }
}
