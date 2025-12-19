namespace Lines.Application.Features.Drivers.GetDriverAddress.DTOs;

public class DriverAddressDto
{
    public string Address { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public string CityName { get; set; } = string.Empty;
    public Guid? SectorId { get; set; }
    public string? SectorName { get; set; }
    public string PostalCode { get; set; } = string.Empty;
}
