using Lines.Domain.Enums;

namespace Lines.Presentation.Endpoints.Trips.GetTrips;

public class GetTripsResponse
{
    public Guid Id { get; set; }
    public string DriverName { get; set; }
    public string Status { get; set; }
    public DateTime? PickUpDate { get; set; }
    public DateTime? DropOffDate { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public string PaymentMethodName { get; set; }
    public int PaymentStatus { get; set; }
    public string PaymentStatusName { get; set; }
}