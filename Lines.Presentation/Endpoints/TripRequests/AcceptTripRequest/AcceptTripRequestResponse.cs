using Lines.Application.Features.Common.DTOs;
using Lines.Application.Features.TripRequests.DTOs;
using Mapster;

namespace Lines.Presentation.Endpoints.TripRequests;

public class AcceptTripRequestResponse
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public LocationDto PickUpLocation { get; set; }
}

public class AcceptTripRequestResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatedTripForDriverDto, AcceptTripRequestResponse>()
            .Map(dest => dest.PassengerId, src => src.PassengerId)
            .Map(dest => dest.PickUpLocation, src => src.PickUpLocation);
    }
}