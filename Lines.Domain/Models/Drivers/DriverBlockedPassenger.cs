using Lines.Domain.Models.Common;
using Lines.Domain.Models.Passengers;

namespace Lines.Domain.Models.Drivers;

public class DriverBlockedPassenger : BaseModel
{
    public Guid DriverId { get; set; }
    public virtual Driver Driver { get; set; }
    
    public Guid PassengerId { get; set; }
    public virtual Passenger Passenger { get; set; }
    
    public string? Reason { get; set; }

    // Parameterless constructor for EF Core
    public DriverBlockedPassenger() { }

    public DriverBlockedPassenger(Guid driverId, Guid passengerId, string? reason = null)
    {
        ValidateBlockingDetails(driverId, passengerId);
        
        DriverId = driverId;
        PassengerId = passengerId;
        Reason = reason;
    }

    private void ValidateBlockingDetails(Guid driverId, Guid passengerId)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("Driver ID cannot be empty", nameof(driverId));
            
        if (passengerId == Guid.Empty)
            throw new ArgumentException("Passenger ID cannot be empty", nameof(passengerId));
            
        if (driverId == passengerId)
            throw new ArgumentException("Driver cannot block themselves", nameof(passengerId));
    }
}

