using Lines.Domain.Models.Common;
using Lines.Domain.Models.Sites;

namespace Lines.Domain.Models.Vehicles;

public class CityVehicleType : BaseModel
{
    public Guid CityId { get; set; }
    public City City { get; set; }

    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; }

    public CityVehicleType()
    {
        
    }

    public CityVehicleType(Guid cityId, Guid vehicleTypeId)
    {
        if (cityId == Guid.Empty) throw new ArgumentException("City ID cannot be empty.", nameof(cityId));
        if (vehicleTypeId == Guid.Empty) throw new ArgumentException("Vehicle Type ID cannot be empty.", nameof(vehicleTypeId));

        CityId = cityId;
        VehicleTypeId = vehicleTypeId;
    }
}