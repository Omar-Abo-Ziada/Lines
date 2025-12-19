using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.TripRequests.DTOs;

public class CreatedTripForDriverDto
{
    public  Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public Location PickUpLocation { get; set; }
}
