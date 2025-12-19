namespace Lines.Application.Features.Trips.GetTrips.DTOs;

public class GetTripsDto
{
    
    public Guid Id { get; set; }
    public string DriverName { get; set; }
    public TripStatus Status { get; set; }
    public DateTime? PickUpDate { get; set; }
    public DateTime? DropOffDate { get; set; }
    public PaymentMethodType? PaymentMethod { get; set; }
    public string PaymentMethodName { get; set; }
    public int PaymentStatus { get; set; }
    public string PaymentStatusName { get; set; }

}