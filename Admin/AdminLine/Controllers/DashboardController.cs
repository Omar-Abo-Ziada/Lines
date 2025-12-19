using AdminLine.Common.DTOs;
using AdminLine.Common.Helper;
using AdminLine.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AdminLine.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<ApiResponse<DashboardOverviewDto>>> GetOverview()
    {
        try
        {
            var dashboardData = await _dashboardService.GetDashboardOverviewAsync();
            return ApiResponse<DashboardOverviewDto>.SuccessResponse(dashboardData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard overview");
            return ApiResponse<DashboardOverviewDto>.ErrorResponse(
                Error.Create("DASHBOARD_ERROR", "Failed to retrieve dashboard data"),
                500);
        }
    }
}

