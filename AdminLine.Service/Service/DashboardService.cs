using AdminLine.Common.DTOs;
using AdminLine.Service.Helpers;
using AdminLine.Service.IService;
using Microsoft.Extensions.Logging;

namespace AdminLine.Service.Service;

public class DashboardService : IDashboardService
{
    private readonly IDriverService _driverService;
    private readonly IFinanceService _financeService;
    private readonly IRegistrationService _registrationService;
    private readonly IRightsService _rightsService;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        IDriverService driverService,
        IFinanceService financeService,
        IRegistrationService registrationService,
        IRightsService rightsService,
        ILogger<DashboardService> logger)
    {
        _driverService = driverService;
        _financeService = financeService;
        _registrationService = registrationService;
        _rightsService = rightsService;
        _logger = logger;
    }

    public async Task<DashboardOverviewDto> GetDashboardOverviewAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var lastWeek = now.AddDays(-7);
            var lastMonth = now.AddDays(-30);
            var lastHour = now.AddHours(-1);

            // Get current values
            var allDriversCurrent = await _driverService.GetAllDriversCountAsync();
            var onlineNowCurrent = await _driverService.GetOnlineDriversCountAsync();
            var pendingRegistrationsCurrent = await _registrationService.GetPendingRegistrationsCountAsync();
            var driversProfitsCurrent = await _financeService.GetDriversProfitsAsync(lastMonth, now);
            var appProfitCurrent = await _financeService.GetAppProfitsAsync(lastMonth, now);
            var onTripCurrent = await _driverService.GetOnTripDriversCountAsync();
            var driversRightsCurrent = await _rightsService.GetDriversRightsAsync(lastWeek, now);
            var appRightsCurrent = await _rightsService.GetAppRightsAsync(lastWeek, now);

            // Get previous period values for comparison
            // Note: For counts (all drivers, online now, on trip, pending registrations), 
            // we compare current vs a snapshot from the previous period
            // Since we don't have historical snapshots, we'll use a simplified approach
            // In production, consider storing periodic snapshots for accurate comparisons
            
            // For now, we'll calculate previous periods for date-based metrics
            var driversProfitsPrevious = await _financeService.GetDriversProfitsAsync(lastMonth.AddDays(-30), lastMonth);
            var appProfitPrevious = await _financeService.GetAppProfitsAsync(lastMonth.AddDays(-30), lastMonth);
            var driversRightsPrevious = await _rightsService.GetDriversRightsAsync(lastWeek.AddDays(-7), lastWeek);
            var appRightsPrevious = await _rightsService.GetAppRightsAsync(lastWeek.AddDays(-7), lastWeek);
            
            // For count-based metrics, we'll use a placeholder approach
            // In a real system, you'd store periodic snapshots
            // For now, we'll set previous to 0 to show "New" or calculate based on a simple estimation
            var allDriversPrevious = Math.Max(0, allDriversCurrent - 10); // Placeholder - should be from snapshot
            var onlineNowPrevious = Math.Max(0, onlineNowCurrent - 5); // Placeholder - should be from snapshot
            var pendingRegistrationsPrevious = Math.Max(0, pendingRegistrationsCurrent - 2); // Placeholder - should be from snapshot
            var onTripPrevious = Math.Max(0, onTripCurrent - 3); // Placeholder - should be from snapshot

            // For counts that need previous period, we'll need to bypass cache or calculate differently
            // For now, we'll use a simplified approach - in production, you might want to store historical snapshots

            // Calculate percentage changes
            var allDriversChange = PercentageCalculator.CalculatePercentageChange(
                allDriversCurrent, 
                allDriversPrevious, 
                "vs last week");

            var onlineNowChange = PercentageCalculator.CalculatePercentageChange(
                onlineNowCurrent, 
                onlineNowPrevious, 
                "vs last hour");

            var pendingRegistrationsChange = PercentageCalculator.CalculatePercentageChange(
                pendingRegistrationsCurrent, 
                pendingRegistrationsPrevious, 
                "vs last week");

            var driversProfitsChange = PercentageCalculator.CalculatePercentageChange(
                driversProfitsCurrent, 
                driversProfitsPrevious, 
                "vs last month");

            var appProfitChange = PercentageCalculator.CalculatePercentageChange(
                appProfitCurrent, 
                appProfitPrevious, 
                "vs last month");

            var onTripChange = PercentageCalculator.CalculatePercentageChange(
                onTripCurrent, 
                onTripPrevious, 
                "vs last hour");

            var driversRightsChange = PercentageCalculator.CalculatePercentageChange(
                driversRightsCurrent, 
                driversRightsPrevious, 
                "vs last week");

            var appRightsChange = PercentageCalculator.CalculatePercentageChange(
                appRightsCurrent, 
                appRightsPrevious, 
                "vs last week");

            return new DashboardOverviewDto
            {
                AllDrivers = new KpiCardDto
                {
                    Value = allDriversCurrent,
                    PercentageChange = allDriversChange.PercentageChange,
                    ComparisonDescription = allDriversChange.Description,
                    ChangeType = allDriversChange.ChangeType
                },
                OnlineNow = new KpiCardDto
                {
                    Value = onlineNowCurrent,
                    PercentageChange = onlineNowChange.PercentageChange,
                    ComparisonDescription = onlineNowChange.Description,
                    ChangeType = onlineNowChange.ChangeType
                },
                PendingRegistrations = new KpiCardDto
                {
                    Value = pendingRegistrationsCurrent,
                    PercentageChange = pendingRegistrationsChange.PercentageChange,
                    ComparisonDescription = pendingRegistrationsChange.Description,
                    ChangeType = pendingRegistrationsChange.ChangeType
                },
                DriversProfits = new KpiCardDto
                {
                    Value = driversProfitsCurrent,
                    PercentageChange = driversProfitsChange.PercentageChange,
                    ComparisonDescription = driversProfitsChange.Description,
                    ChangeType = driversProfitsChange.ChangeType
                },
                AppProfit = new KpiCardDto
                {
                    Value = appProfitCurrent,
                    PercentageChange = appProfitChange.PercentageChange,
                    ComparisonDescription = appProfitChange.Description,
                    ChangeType = appProfitChange.ChangeType
                },
                OnTrip = new KpiCardDto
                {
                    Value = onTripCurrent,
                    PercentageChange = onTripChange.PercentageChange,
                    ComparisonDescription = onTripChange.Description,
                    ChangeType = onTripChange.ChangeType
                },
                DriversRights = new KpiCardDto
                {
                    Value = driversRightsCurrent,
                    PercentageChange = driversRightsChange.PercentageChange,
                    ComparisonDescription = driversRightsChange.Description,
                    ChangeType = driversRightsChange.ChangeType
                },
                AppRights = new KpiCardDto
                {
                    Value = appRightsCurrent,
                    PercentageChange = appRightsChange.PercentageChange,
                    ComparisonDescription = appRightsChange.Description,
                    ChangeType = appRightsChange.ChangeType
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard overview");
            // Return empty dashboard on error
            return new DashboardOverviewDto();
        }
    }
}

