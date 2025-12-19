using Lines.Application.Features.Common.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.Offers.Accept_Offer;

public class AcceptOfferResponse
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public required LocationDto PickUpLocation { get; set; }
}

public class AcceptOfferResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<CreatedTripForDriverDto, AcceptOfferResponse>()
        //    .Map(dest => dest.PassengerId, src => src.PassengerId)
        //    .Map(dest => dest.PickUpLocation, src => src.PickUpLocation)
        //    .Map(dest => dest.PickUpLocation, src => src.PickUpLocation);
    }
}