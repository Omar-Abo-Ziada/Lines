using Lines.Application.Features.DriverStatistics.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetWeeklyStatistics;

public record DayValueResponse(string Day, double Value);

public record GetWeeklyStatisticsResponse(
    List<DayValueResponse> DistancePerDay,
    double TotalDistanceWeek,
    string PeakDay,
    string MinDay,
    List<DayValueResponse> IncomePerDay,
    decimal TotalIncomeWeek,
    string PeakIncomeDay,
    string MinIncomeDay,
    List<DayValueResponse> TripsPerDay,
    int TotalTripsWeek,
    string PeakTripsDay,
    string MinTripsDay,
    List<DayValueResponse> NetProfitPerDay,
    decimal TotalNetProfitWeek,
    string PeakNetProfitDay,
    string MinNetProfitDay,
    List<DayValueResponse> WorkingHoursPerDay,
    double TotalWorkingHoursWeek,
    string PeakWorkingHoursDay,
    string MinWorkingHoursDay);

public class GetWeeklyStatisticsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DayValueDto, DayValueResponse>()
            .Map(dest => dest.Day, src => src.Day)
            .Map(dest => dest.Value, src => src.Value);

        config.NewConfig<WeeklyStatisticsDto, GetWeeklyStatisticsResponse>()
            .Map(dest => dest.DistancePerDay, src => src.DistancePerDay)
            .Map(dest => dest.TotalDistanceWeek, src => src.TotalDistanceWeek)
            .Map(dest => dest.PeakDay, src => src.PeakDay)
            .Map(dest => dest.MinDay, src => src.MinDay)
            .Map(dest => dest.IncomePerDay, src => src.IncomePerDay)
            .Map(dest => dest.TotalIncomeWeek, src => src.TotalIncomeWeek)
            .Map(dest => dest.PeakIncomeDay, src => src.PeakIncomeDay)
            .Map(dest => dest.MinIncomeDay, src => src.MinIncomeDay)
            .Map(dest => dest.TripsPerDay, src => src.TripsPerDay)
            .Map(dest => dest.TotalTripsWeek, src => src.TotalTripsWeek)
            .Map(dest => dest.PeakTripsDay, src => src.PeakTripsDay)
            .Map(dest => dest.MinTripsDay, src => src.MinTripsDay)
            .Map(dest => dest.NetProfitPerDay, src => src.NetProfitPerDay)
            .Map(dest => dest.TotalNetProfitWeek, src => src.TotalNetProfitWeek)
            .Map(dest => dest.PeakNetProfitDay, src => src.PeakNetProfitDay)
            .Map(dest => dest.MinNetProfitDay, src => src.MinNetProfitDay)
            .Map(dest => dest.WorkingHoursPerDay, src => src.WorkingHoursPerDay)
            .Map(dest => dest.TotalWorkingHoursWeek, src => src.TotalWorkingHoursWeek)
            .Map(dest => dest.PeakWorkingHoursDay, src => src.PeakWorkingHoursDay)
            .Map(dest => dest.MinWorkingHoursDay, src => src.MinWorkingHoursDay);
    }
}


