//using Lines.Application.Features.Common.DTOs;
//using Lines.Domain.Models.Trips;
//using Lines.Domain.Value_Objects;
//using Mapster;

//namespace Lines.Application.Features.TripRequests.DTOs;

//public class CreateTripRequestDto
//{
//    public Guid Id { get; set; }

//    // Timing
//    public DateTime RequestedAt { get; set; }
//    public DateTime? EstimatedPickupTime { get; set; }
//    public bool IsScheduled { get; set; }
//    public DateTime? ScheduledAt { get; set; }

//    // Status & Pricing
//    public string Status { get; set; }   // TripRequestStatus as string (or use enum if you prefer)
//    public decimal EstimatedPrice { get; set; }
//    public double Distance { get; set; }

//    // Driver
//    public Guid? DriverId { get; set; }

//    // Passenger
//    public Guid PassengerId { get; set; }

//    // Locations
//    public LocationDto StartLocation { get; set; }
//    public List<LocationDto> EndLocations { get; set; }

//    // Vehicle & Payment
//    public Guid VehicleTypeId { get; set; }
//    public Guid PaymentMethodId { get; set; }

//    // Optional linked Trip
//    public Guid? TripId { get; set; }

//    public bool IsAnonymous { get; set; }
//}


//public class CreateTripRequestDtoMappingConfig : IRegister
//{
//    public void Register(TypeAdapterConfig config)
//    {

//        config.NewConfig<TripRequest, CreateTripRequestDto>()
//            .Map(dest => dest.Status, src => src.Status.ToString()) 
//            .Map(dest => dest.EndLocations, src => src.EndLocations); 
//    }
//}
