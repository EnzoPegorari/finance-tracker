using FinanceTracker.Api.Helpers;
using FinanceTracker.Api.Models.DTOs.Dashboard;
using FinanceTracker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var summary = await _dashboardService.GetSummaryAsync(User.GetUserId());
        return Ok(summary);
    }

    [HttpGet("by-category")]
    public async Task<ActionResult<List<CategoryBreakdownDto>>> GetByCategory([FromQuery] int month, [FromQuery] int year)
    {
        var breakdown = await _dashboardService.GetByCategoryAsync(User.GetUserId(), month, year);
        return Ok(breakdown);
    }

    [HttpGet("balance-history")]
    public async Task<ActionResult<List<BalanceHistoryPointDto>>> GetBalanceHistory([FromQuery] int months = 6)
    {
        var history = await _dashboardService.GetBalanceHistoryAsync(User.GetUserId(), months);
        return Ok(history);
    }
}
