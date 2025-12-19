using Lines.Application.Features.Drivers.GetDriverTrips.DTOs;
using Lines.Application.Shared;
using Mapster;

namespace Lines.Presentation.Endpoints.Drivers.Dashboard.GetTrips;

public record GetDriverTripsResponse(
    List<DriverTripItemResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record DriverTripItemResponse(
    Guid TripId,
    string RiderName,
    double Rating,
    DateTime? PickUpTime,
    decimal TripCost,
    string Currency,
    string PaymentMethod,
    string PaymentStatus,
    string TripStatus,
    double? TripRate,
    string? Comment
);

public class GetDriverTripsResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PagingDto<DriverTripDto>, GetDriverTripsResponse>()
            .Map(dest => dest.Items, src => src.Items)
            .Map(dest => dest.PageNumber, src => src.PageNumber)
            .Map(dest => dest.PageSize, src => src.PageSize)
            .Map(dest => dest.TotalCount, src => src.TotalCount)
            .Map(dest => dest.TotalPages, src => src.TotalPages);

        config.NewConfig<DriverTripDto, DriverTripItemResponse>()
            .Map(dest => dest.TripId, src => src.TripId)
            .Map(dest => dest.RiderName, src => src.RiderName)
            .Map(dest => dest.Rating, src => src.Rating)
            .Map(dest => dest.PickUpTime, src => src.PickUpTime)
            .Map(dest => dest.TripCost, src => src.TripCost)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod)
            .Map(dest => dest.PaymentStatus, src => src.PaymentStatus)
            .Map(dest => dest.TripStatus, src => src.TripStatus)
            .Map(dest => dest.TripRate, src => src.TripRate)
            .Map(dest => dest.Comment, src => src.Comment);
    }
}
