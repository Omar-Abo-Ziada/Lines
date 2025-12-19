namespace Lines.Application.Features.Drivers.UpdateDriverAddress.DTOs;

public class UpdateDriverAddressDto
{
    public string Address { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public Guid? SectorId { get; set; }
    public string PostalCode { get; set; } = string.Empty;
}
