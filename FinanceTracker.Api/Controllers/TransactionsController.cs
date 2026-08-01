using FinanceTracker.Api.Helpers;
using FinanceTracker.Api.Models.DTOs;
using FinanceTracker.Api.Models.DTOs.Transactions;
using FinanceTracker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TransactionDto>>> GetAll([FromQuery] TransactionFilterRequest filter)
    {
        var result = await _transactionService.GetFilteredAsync(User.GetUserId(), filter);
        return Ok(result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var csvBytes = await _transactionService.ExportCsvAsync(User.GetUserId(), from, to);
        return File(csvBytes, "text/csv", "transactions.csv");
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(CreateTransactionRequest request)
    {
        var transaction = await _transactionService.CreateAsync(User.GetUserId(), request);
        return CreatedAtAction(nameof(GetAll), new { }, transaction);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> Update(Guid id, UpdateTransactionRequest request)
    {
        var transaction = await _transactionService.UpdateAsync(User.GetUserId(), id, request);
        return Ok(transaction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _transactionService.DeleteAsync(User.GetUserId(), id);
        return NoContent();
    }
}
