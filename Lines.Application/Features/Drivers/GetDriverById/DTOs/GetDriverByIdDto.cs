using Lines.Domain.Models.Drivers;
using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.Drivers.GetDriverById.DTOs
{
    public class GetDriverByIdDto
    {
        public bool IsAvailable { get; set; } = true; 
        public bool IsNotifiedForOnlyTripsAboveMyPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalTrips { get; set; }
        public int RatedTripsCount { get; set; }
        public double Rating { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
    }


    public class GetDriverByIdDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetDriverByIdDto, Driver>()
                .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                .Map(dest => dest.IsNotifiedForOnlyTripsAboveMyPrice, src => src.IsNotifiedForOnlyTripsAboveMyPrice)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.TotalTrips, src => src.TotalTrips)
                .Map(dest => dest.RatedTripsCount, src => src.RatedTripsCount)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);


            config.NewConfig<Driver, GetDriverByIdDto>()
                .Map(dest => dest.IsAvailable, src => src.IsAvailable)
                .Map(dest => dest.IsNotifiedForOnlyTripsAboveMyPrice, src => src.IsNotifiedForOnlyTripsAboveMyPrice)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.TotalTrips, src => src.TotalTrips)
                .Map(dest => dest.RatedTripsCount, src => src.RatedTripsCount)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);
        }
    }
}
