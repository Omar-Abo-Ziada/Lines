namespace Lines.Application.Features.Trips.GetTripInvoice.DTOs;

public class TripInvoiceDto
{
    public Guid TripId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    
    // Driver info
    public string DriverName { get; set; } = string.Empty;
    public string DriverPhoneNumber { get; set; } = string.Empty;
    
    // Passenger info
    public string PassengerName { get; set; } = string.Empty;
    public string PassengerPhoneNumber { get; set; } = string.Empty;
    
    // Trip details
    public DateTime? PickUpTime { get; set; }
    public DateTime? DropOffTime { get; set; }
    public string StartLocationAddress { get; set; } = string.Empty;
    public string EndLocationAddress { get; set; } = string.Empty;
    public double? Distance { get; set; }
    
    // Payment details
    public decimal Fare { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
}
