namespace Lines.Application.Features.DriverStatistics.DTOs;

public class DayValueDto
{
    public string Day { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class WeeklyStatisticsDto
{
    public List<DayValueDto> DistancePerDay { get; set; } = new();
    public double TotalDistanceWeek { get; set; }
    public string PeakDay { get; set; } = string.Empty;
    public string MinDay { get; set; } = string.Empty;

    public List<DayValueDto> IncomePerDay { get; set; } = new();
    public decimal TotalIncomeWeek { get; set; }
    public string PeakIncomeDay { get; set; } = string.Empty;
    public string MinIncomeDay { get; set; } = string.Empty;

    public List<DayValueDto> TripsPerDay { get; set; } = new();
    public int TotalTripsWeek { get; set; }
    public string PeakTripsDay { get; set; } = string.Empty;
    public string MinTripsDay { get; set; } = string.Empty;

    public List<DayValueDto> NetProfitPerDay { get; set; } = new();
    public decimal TotalNetProfitWeek { get; set; }
    public string PeakNetProfitDay { get; set; } = string.Empty;
    public string MinNetProfitDay { get; set; } = string.Empty;

    public List<DayValueDto> WorkingHoursPerDay { get; set; } = new();
    public double TotalWorkingHoursWeek { get; set; }
    public string PeakWorkingHoursDay { get; set; } = string.Empty;
    public string MinWorkingHoursDay { get; set; } = string.Empty;
}


