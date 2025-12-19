namespace Lines.Application.Features.Vehicles.GetVehicleTypeIdByDriverId.DTOs
{
    public class VehicleTypeIdDto
    {
        public Guid DriverId { get; set; }
        public Guid VehicleTypeId { get; set; }

        public VehicleTypeIdDto(Guid driverId, Guid vehicleTypeId)
        {
            DriverId = driverId;
            VehicleTypeId = vehicleTypeId;
        }
    }
}
