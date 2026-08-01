namespace FinanceTracker.Api.Models.DTOs.Transactions;

public record TransactionDto(
    Guid Id,
    string Description,
    decimal Amount,
    string Type,
    DateOnly Date,
    string? Notes,
    Guid CategoryId,
    string CategoryName,
    string CategoryColor,
    string CategoryIcon);

public record CreateTransactionRequest(
    string Description,
    decimal Amount,
    string Type,
    DateOnly Date,
    Guid CategoryId,
    string? Notes);

public record UpdateTransactionRequest(
    string Description,
    decimal Amount,
    string Type,
    DateOnly Date,
    Guid CategoryId,
    string? Notes);
