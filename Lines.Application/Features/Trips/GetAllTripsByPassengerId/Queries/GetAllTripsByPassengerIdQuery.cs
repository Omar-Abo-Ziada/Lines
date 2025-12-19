using Lines.Application.Features.Trips.GetAllTripsByPassengerId.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Trips.GetAllTripsByPassengerId.Queries
{
    public record GetAllTripsByPassengerIdQuery(Guid passengerId, Guid userId) : IRequest<List<GetAllTripsByPassengerIdDto>>;
    

    public class GetAllTripsByPassengerIdQueryHandler(IRepository<Trip> _tripRepository, RequestHandlerBaseParameters parameters) 
                    : RequestHandlerBase<GetAllTripsByPassengerIdQuery, List<GetAllTripsByPassengerIdDto>>(parameters)
    {
        public override async Task<List<GetAllTripsByPassengerIdDto>> Handle(GetAllTripsByPassengerIdQuery request, CancellationToken cancellationToken)
        {
            var trips = await _tripRepository.Get(t => t.PassengerId == request.passengerId)
           .Select(t => new GetAllTripsByPassengerIdDto
           {
               TripId = t.Id,
               TripCode = t.TripCode,
               DriverName = t.Driver.FirstName + " " + t.Driver.LastName,
               DriverRate = t.Driver.Rating,
               TripStatus = t.Status,
               TripCost = t.Fare,
               PaymentMethod = t.PaymentMethod.Type.ToString(),
               PaymentStatus = t.IsPaid ? "Paid" : "Unpaid",
               PickupTime = t.EstimatedPickupTime ?? null,
               CancellationReason = t.TripRequest.CancellationReason,
               FeedbackFromYou = t.Feedbacks.FirstOrDefault
                                 (f => f.FromUserId == request.userId && f.ToUserId == t.DriverId && f.TripId == t.Id).Comment,
               RateFromYou = t.Feedbacks.FirstOrDefault
                             (f => f.FromUserId == request.userId && f.ToUserId == t.DriverId && f.TripId == t.Id).Rating,
               FeedbackToYou = t.Feedbacks.FirstOrDefault
                                 (f => f.FromUserId == t.DriverId && f.ToUserId == request.userId && f.TripId == t.Id).Comment,
               RateToYou = t.Feedbacks.FirstOrDefault
               (f => f.FromUserId == t.DriverId && f.ToUserId == request.userId && f.TripId == t.Id).Rating
           })
           .AsNoTracking()
           .ToListAsync(cancellationToken);

            return trips;
        }
    }
}
