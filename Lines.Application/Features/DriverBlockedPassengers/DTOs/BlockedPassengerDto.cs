namespace Lines.Application.Features.DriverBlockedPassengers.DTOs;

public class BlockedPassengerDto
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public double Rating { get; set; }
    public int TotalTrips { get; set; }
    public string? Reason { get; set; }
    public DateTime BlockedDate { get; set; }
}

