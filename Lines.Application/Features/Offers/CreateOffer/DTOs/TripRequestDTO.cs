using Lines.Application.Features.Common.DTOs;

namespace Lines.Application.Features.Offers.CreateOffer.DTOs;
public class TripRequestDTO
{
    public DateTime RequestedAt { get; set; }
    public TripRequestStatus Status { get; set; }

    public bool IsScheduled { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public decimal EstimatedPrice { get; set; }
    public double Distance { get; set; }

    public virtual ICollection<OfferDTO>? Offers { get; set; }

    // Optional: Assigned Driver (when a driver accepts the request)
    public Guid? DriverId { get; set; }
    //public virtual Driver? Driver { get; set; }

    // Requester
    public Guid PassengerId { get; set; }
    //public virtual Passenger Passenger { get; set; }

    // Request Details
    public LocationDto StartLocation { get; set; }
    //public virtual ICollection<EndTripLocation> EndLocations { get; set; }

    public Guid VehicleTypeId { get; set; }
    //public virtual VehicleType VehicleType { get; set; }

    public Guid PaymentMethodId { get; set; }
    //public virtual PaymentMethod PaymentMethod { get; set; }

    public Guid? TripId { get; set; }
    //public virtual Trip Trip { get; set; }

    public bool IsAnonymous { get; set; } = false;
    public Guid? UserRewardId { get; set; }
}

public class tripRequestDTOMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TripRequest, TripRequestDTO>()
            .Map(dest => dest.RequestedAt, src => src.RequestedAt)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.IsScheduled, src => src.IsScheduled)
            .Map(dest => dest.ScheduledAt, src => src.ScheduledAt)
            .Map(dest => dest.EstimatedPrice, src => src.EstimatedPrice)
            .Map(dest => dest.Distance, src => src.Distance)
            .Map(dest => dest.Offers, src => src.Offers)
            .Map(dest => dest.DriverId, src => src.DriverId)
            .Map(dest => dest.PassengerId, src => src.PassengerId)
            .Map(dest => dest.StartLocation, src => src.StartLocation)
            //.Map(dest => dest.EndLocations, src => src.EndLocations)
            .Map(dest => dest.VehicleTypeId, src => src.VehicleTypeId)
            .Map(dest => dest.PaymentMethodId, src => src.PaymentMethodId)
            .Map(dest => dest.TripId, src => src.TripId)
            .Map(dest => dest.UserRewardId, src => src.UserRewardId);
        //.Map(dest => dest.VehicleType, src => src.VehicleType);
    }
}