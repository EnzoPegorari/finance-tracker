using FinanceTracker.Api.Models.Entities;
using FluentValidation;

namespace FinanceTracker.Api.Models.DTOs.Transactions;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).Must(t => t is TransactionType.Income or TransactionType.Expense)
            .WithMessage("Type must be 'income' or 'expense'.");
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}

public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).Must(t => t is TransactionType.Income or TransactionType.Expense)
            .WithMessage("Type must be 'income' or 'expense'.");
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}
