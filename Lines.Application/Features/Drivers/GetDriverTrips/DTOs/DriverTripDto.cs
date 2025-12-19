using Lines.Domain.Enums;

namespace Lines.Application.Features.Drivers.GetDriverTrips.DTOs;

public class DriverTripDto
{
    public Guid TripId { get; set; }
    public string RiderName { get; set; } = string.Empty;
    public double Rating { get; set; } // driver's overall rating
    public DateTime? PickUpTime { get; set; }
    public decimal TripCost { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string TripStatus { get; set; } = string.Empty;
    public double? TripRate { get; set; } // passenger's rating for this trip
    public string? Comment { get; set; }
}
