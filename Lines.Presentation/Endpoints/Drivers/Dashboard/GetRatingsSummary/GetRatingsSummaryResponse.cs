using Lines.Application.Features.Drivers.GetDriverRatingsSummary.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetRatingsSummary;

public record GetRatingsSummaryResponse(
    int TotalRates,
    double AverageRating,
    List<RatingRangeResponse> Rates
);

public record RatingRangeResponse(string Range, int Count);

public class GetRatingsSummaryResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DriverRatingsSummaryDto, GetRatingsSummaryResponse>()
            .Map(dest => dest.TotalRates, src => src.TotalRates)
            .Map(dest => dest.AverageRating, src => src.AverageRating)
            .Map(dest => dest.Rates, src => src.Rates);

        config.NewConfig<RatingRangeDto, RatingRangeResponse>()
            .Map(dest => dest.Range, src => src.Range)
            .Map(dest => dest.Count, src => src.Count);
    }
}
