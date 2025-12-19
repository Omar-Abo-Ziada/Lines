namespace AdminLine.Common.DTOs;

public class DriverProfileHeaderDto
{
    public Guid DriverId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public double? Rating { get; set; }
    public bool IsOnline { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int EmergencyCallsCount { get; set; }
    public bool IsVerified { get; set; }
}
