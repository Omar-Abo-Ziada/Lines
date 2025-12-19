using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Offers.CreateOffer;

public class CreateOfferReponse
{
    public int TimeToArriveInMinutes { get; set; }
    public float DistanceToArriveInMeters { get; set; }
    public float Price { get; set; }

    public Guid DriverId { get; set; }
    //public virtual Driver Driver { get; set; }

    // Status
    public bool IsAccepted { get; set; }
    public Guid TripRequestId { get; set; }
    //public virtual TripRequest TripRequest { get; set; }
}

public class CreateOfferReponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OfferDTO, CreateOfferReponse>()
            .Map(dest => dest.TimeToArriveInMinutes, src => src.TimeToArriveInMinutes)
            .Map(dest => dest.DistanceToArriveInMeters, src => src.DistanceToArriveInMeters)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.DriverId, src => src.DriverId)
            .Map(dest => dest.IsAccepted, src => src.IsAccepted)
            .Map(dest => dest.TripRequestId, src => src.TripRequestId);
    }
}