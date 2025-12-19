using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Offers.CreateOffer.DTOs;
public class OfferDTO
{
    public int TimeToArriveInMinutes { get; set; }
    public float DistanceToArriveInMeters { get; set; }
    public float Price { get; set; }

    public Guid DriverId { get; set; }
    //public virtual Driver Driver { get; set; }

    // Status
    public bool IsAccepted { get; set; }
    public Guid TripRequestId { get; set; }
    //public virtual TripRequestDTO TripRequest { get; set; }
}

public class OfferDTOMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Offer, OfferDTO>()
            .Map(dest => dest.TimeToArriveInMinutes, src => src.TimeToArriveInMinutes)
            .Map(dest => dest.DistanceToArriveInMeters, src => src.DistanceToArriveInMeters)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.DriverId, src => src.DriverId)
            .Map(dest => dest.IsAccepted, src => src.IsAccepted)
            //.Map(dest => dest.TripRequest, src => src.TripRequest)
            .Map(dest => dest.TripRequestId, src => src.TripRequestId);
    }
}