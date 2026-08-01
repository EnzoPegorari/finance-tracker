namespace FinanceTracker.Api.Models.DTOs.Transactions;

public class TransactionFilterRequest
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Type { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
