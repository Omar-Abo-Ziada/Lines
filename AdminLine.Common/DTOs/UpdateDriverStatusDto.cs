namespace AdminLine.Common.DTOs;

public class UpdateDriverStatusDto
{
    public Guid DriverId { get; set; }
    public DriverStatus Status { get; set; }
    public string? Reason { get; set; }
}

