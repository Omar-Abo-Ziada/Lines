namespace Lines.Application.Features.TripRequests.DTOs;

public class CreatedTripDto
{
    public  Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public string DriverName { get; set; }
    public Guid PassengerId { get; set; }
    public DateTime? EstimatedPickupTime { get; set; }
    public Location  DriverLocation { get; set; }
    public decimal Fare { get; set; }
    public decimal? FareAfterRewardApplied { get; set; }
}

public class CreatedTripDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Trip, CreatedTripDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.DriverId, src => src.DriverId)
            .Map(dest => dest.EstimatedPickupTime, src => src.EstimatedPickupTime)
            .Map(dest => dest.PassengerId, src => src.PassengerId)
            .Map(dest => dest.Fare, src => src.Fare)
            .Map(dest => dest.FareAfterRewardApplied, src => src.FareAfterRewardApplied);
        //.Map(dest => dest.DriverName, src => src.Driver.FirstName + " " + src.Driver.LastName)
        // .Map(dest => dest.DriverLocation, src => src.Driver.CurrentLocation);
    }
}