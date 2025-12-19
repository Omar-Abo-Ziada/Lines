namespace AdminLine.Common.DTOs;

public class DriverDashboardDto
{
    public int CompletedTripsCount { get; set; }
    public int CanceledTripsCount { get; set; }
    public int ScheduledTripsCount { get; set; }
    public int ActiveTripsCount { get; set; }
    public decimal DriverProfit { get; set; }
    public decimal AppProfit { get; set; }
    public decimal DriverDues { get; set; }
    public decimal AppDues { get; set; }
}
