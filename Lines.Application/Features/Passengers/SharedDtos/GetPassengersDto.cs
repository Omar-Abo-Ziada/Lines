using Lines.Domain.Models.Passengers;

namespace Lines.Application.Features.Passengers.SharedDtos
{
    public class GetPassengersDto 
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalTrips { get; set; }
        public double Rating { get; set; }
        public int RatedTripsCount { get; set; }
        public string ReferralCode { get; set; }
        public int Points { get; set; }

    }



    public class GetAllPassengersDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Passenger, GetPassengersDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.TotalTrips, src => src.TotalTrips)
                .Map(dest => dest.Rating, src => src.Rating)
                .Map(dest => dest.RatedTripsCount, src => src.RatedTripsCount)
                .Map(dest => dest.ReferralCode, src => src.ReferralCode)
                .Map(dest => dest.Points, src => src.Points);
        }
    }
}
