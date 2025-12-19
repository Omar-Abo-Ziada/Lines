namespace Lines.Application.Features.TripRequests.DTOs;

public class GetTripRequestForAcceptDto
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public TripRequestStatus Status { get; set; }
    public Location StartLocation { get; set; }
    public decimal EstimatedPrice { get; set; }
    public double Distance { get; set; }
    public Guid VehicleTypeId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public Guid? UserRewardId { get; set; }
}

public class GetTripRequestForAcceptDtoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TripRequest, GetTripRequestForAcceptDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.PassengerId, src => src.PassengerId)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.EstimatedPrice, src => src.EstimatedPrice)
            .Map(dest => dest.Distance, src => src.Distance)
            .Map(dest => dest.PaymentMethodId, src => src.PaymentMethodId)
            .Map(dest => dest.VehicleTypeId, src => src.VehicleTypeId)
            .Map(dest => dest.StartLocation, src => src.StartLocation)
            .Map(dest => dest.UserRewardId, src => src.UserRewardId);
    }
}