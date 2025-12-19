namespace Lines.Application.Features.DriverStatistics.DTOs;

public class DailyStatisticsDto
{
    public double DistanceKm { get; set; }
    public decimal IncomeChf { get; set; }
    public decimal NetProfitChf { get; set; }
    public int NumberOfTrips { get; set; }
    public decimal AvgIncomePerTrip { get; set; }
    public decimal AppRightsChf { get; set; }
    public string Availability { get; set; } = "00:00";
    public decimal AvgProfitLastTrip { get; set; }
    public decimal TipsChf { get; set; }
}


