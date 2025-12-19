using Lines.Application.Features.DriverStatistics.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDailyStatistics;

public record GetDailyStatisticsResponse(
    double DistanceKm,
    decimal IncomeChf,
    decimal NetProfitChf,
    int NumberOfTrips,
    decimal AvgIncomePerTrip,
    decimal AppRightsChf,
    string Availability,
    decimal AvgProfitLastTrip,
    decimal TipsChf);

public class GetDailyStatisticsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DailyStatisticsDto, GetDailyStatisticsResponse>()
            .Map(dest => dest.DistanceKm, src => src.DistanceKm)
            .Map(dest => dest.IncomeChf, src => src.IncomeChf)
            .Map(dest => dest.NetProfitChf, src => src.NetProfitChf)
            .Map(dest => dest.NumberOfTrips, src => src.NumberOfTrips)
            .Map(dest => dest.AvgIncomePerTrip, src => src.AvgIncomePerTrip)
            .Map(dest => dest.AppRightsChf, src => src.AppRightsChf)
            .Map(dest => dest.Availability, src => src.Availability)
            .Map(dest => dest.AvgProfitLastTrip, src => src.AvgProfitLastTrip)
            .Map(dest => dest.TipsChf, src => src.TipsChf);
    }
}


