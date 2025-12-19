namespace Lines.Application.Features.Drivers.GetNearbyDriverConnectionsFilteredByKmPrice.DTOs
{
    public class VehicleKmPriceDto
    {
        public Guid DriverId { get; set; }
        public decimal KmPrice { get; set; }

        public VehicleKmPriceDto(Guid driverId, decimal kmPrice)
        {
            DriverId = driverId;
            KmPrice = kmPrice;
        }
    }
}
