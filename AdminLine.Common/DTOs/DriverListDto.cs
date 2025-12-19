namespace AdminLine.Common.DTOs;

public class DriverListDto
{
    public Guid DriverId { get; set; }
    public string DriverCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DriverStatus Status { get; set; }
    public int? TotalTrips { get; set; }
    public decimal? TotalEarnings { get; set; }
    public double? Rating { get; set; }
    public string? LastActivity { get; set; }
}

