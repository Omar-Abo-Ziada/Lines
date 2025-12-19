namespace Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs
{
    public class GetPassengerAndDriverIdsDTO
    {
        public Guid UserId { get; set; }
        public Guid? PassengerId { get; set; } 
        public Guid? DriverId { get; set; }
        public bool IsDeleted { get; set; }

        public GetPassengerAndDriverIdsDTO(Guid userId, Guid? passengerId , Guid? driverId , bool isDeleted = false)
        {
            UserId = userId;
            PassengerId = passengerId;
            DriverId = driverId;
            IsDeleted = isDeleted;
        }
    }
}
