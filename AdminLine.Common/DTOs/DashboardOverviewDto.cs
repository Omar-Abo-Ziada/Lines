namespace AdminLine.Common.DTOs;

public class DashboardOverviewDto
{
    public KpiCardDto AllDrivers { get; set; } = new();
    public KpiCardDto OnlineNow { get; set; } = new();
    public KpiCardDto PendingRegistrations { get; set; } = new();
    public KpiCardDto DriversProfits { get; set; } = new();
    public KpiCardDto AppProfit { get; set; } = new();
    public KpiCardDto OnTrip { get; set; } = new();
    public KpiCardDto DriversRights { get; set; } = new();
    public KpiCardDto AppRights { get; set; } = new();
}

