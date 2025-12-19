using Lines.Application.Features.Passengers.SharedDtos;
using Lines.Presentation.Endpoints.Passengers.GetAllPassengers;

namespace Lines.Presentation.Endpoints.Passengers.GetById
{
    public record GetPassengerByIdResponse (Guid Id, string FirstName, string LastName, string Email, string PhoneNumber, int RideCount);


    public class GetPassengerByIdResponseMappingConfig : Mapster.IRegister
    {
        public void Register(Mapster.TypeAdapterConfig config)
        {
            config.NewConfig<GetPassengersDto, GetAllPassengersResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.TotalTrips, src => src.TotalTrips);

        }
    }
}
