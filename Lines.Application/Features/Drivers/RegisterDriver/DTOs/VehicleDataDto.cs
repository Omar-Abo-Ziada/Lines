namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public class VehicleDataDto
{
    public Guid VehicleTypeId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public decimal KmPrice { get; set; }
    public List<string>? DocumentUrls { get; set; }
}
