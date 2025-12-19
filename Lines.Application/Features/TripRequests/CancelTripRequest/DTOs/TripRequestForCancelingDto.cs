namespace Lines.Application.Features.TripRequests.DTOs;

public class TripRequestForCancelingDto
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public TripRequestStatus Status { get;  set; }
    public Location StartLocation { get;  set; }
    public decimal EstimatedPrice { get;  set; }
    public float Distance { get; set; }
    public Guid VehicleTypeId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public Guid? UserRewardId { get; set; }
}