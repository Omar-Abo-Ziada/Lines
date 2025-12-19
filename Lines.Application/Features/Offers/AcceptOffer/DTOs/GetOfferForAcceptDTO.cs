using Lines.Application.Features.TripRequests.DTOs;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Offers.AcceptOffer.DTOs;
public class GetOfferForAcceptDTO
{
    public int TimeToArriveInMinutes { get; set; }
    public float DistanceToArriveInMeters { get; set; }
    public float Price { get; set; }

    public Guid DriverId { get; set; }

    // Status
    public bool IsAccepted { get; set; }
    public Guid TripRequestId { get; set; }
    public GetTripRequestForAcceptDto? TripRequest { get; set; }
}

public class GetOfferForAcceptDTOMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Offer, GetOfferForAcceptDTO>()
            .Map(dest => dest.TimeToArriveInMinutes, src => src.TimeToArriveInMinutes)
            .Map(dest => dest.DistanceToArriveInMeters, src => src.DistanceToArriveInMeters)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.DriverId, src => src.DriverId)
            .Map(dest => dest.IsAccepted, src => src.IsAccepted)
            .Map(dest => dest.TripRequestId, src => src.TripRequestId)
            // Nested mapping for TripRequest using the existing mapping config
            .Map(dest => dest.TripRequest, src => src.TripRequest.Adapt<GetTripRequestForAcceptDto>());
    }
}