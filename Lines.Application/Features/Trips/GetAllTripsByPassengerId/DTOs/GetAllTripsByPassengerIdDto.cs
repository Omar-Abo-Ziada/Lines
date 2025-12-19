namespace Lines.Application.Features.Trips.GetAllTripsByPassengerId.DTOs
{
    public class GetAllTripsByPassengerIdDto   
    {
        public Guid TripId { get; set; }
        public string? TripCode { get; set; }
        public string DriverName { get; set; }
        public double DriverRate { get; set; }
        public TripStatus TripStatus { get; set; }
        public decimal TripCost { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } 
        public DateTime? PickupTime { get; set; }
        public int? RateFromYou { get; set; }
        public string? FeedbackFromYou { get; set; }
        public int? RateToYou { get; set; }
        public string? FeedbackToYou { get; set; }
        public string? CancellationReason { get; set; }

    }


    public class GetAllTripsByPassengerIdDtoConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Trip, GetAllTripsByPassengerIdDto>()
                .Map(dest => dest.TripId, src => src.Id)
                .Map(dest => dest.DriverName, src => src.Driver.FirstName + " " + src.Driver.LastName)
                .Map(dest => dest.DriverRate, src => src.Driver.Rating)
                .Map(dest => dest.TripStatus, src => src.Status)
                .Map(dest => dest.TripCost, src => src.Fare)
                .Map(dest => dest.PaymentMethod, src => src.PaymentMethod.Type.ToString())
                .Map(dest => dest.PaymentStatus, src => src.IsPaid ? "Paid" : "Unpaid") 
                .Map(dest => dest.PickupTime, src => src.EstimatedPickupTime.HasValue ? src.EstimatedPickupTime.Value : DateTime.MinValue)
                .Map(dest => dest.CancellationReason, src => src.TripRequest.CancellationReason);
        }
    }
}
