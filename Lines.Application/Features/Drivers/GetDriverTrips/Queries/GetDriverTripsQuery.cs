using Application.Common.Helpers;
using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetDriverTrips.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Enums;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverTrips.Queries;

public record GetDriverTripsQuery(
    Guid DriverId,
    int? TripStatus,
    DateTime? DateRangeStart,
    DateTime? DateRangeEnd,
    int? PaymentStatus,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagingDto<DriverTripDto>>;

public class GetDriverTripsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
    : RequestHandlerBase<GetDriverTripsQuery, PagingDto<DriverTripDto>>(parameters)
{
    public override async Task<PagingDto<DriverTripDto>> Handle(GetDriverTripsQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Trip>(true);
        
        // Filter by driver
        predicate = predicate.And(x => x.DriverId == request.DriverId);

        // Filter by trip status
        if (request.TripStatus.HasValue)
        {
            predicate = predicate.And(x => x.Status == (TripStatus)request.TripStatus.Value);
        }

        // Filter by date range
        if (request.DateRangeStart.HasValue)
        {
            predicate = predicate.And(x => x.StartedAt >= request.DateRangeStart.Value);
        }

        if (request.DateRangeEnd.HasValue)
        {
            predicate = predicate.And(x => x.StartedAt <= request.DateRangeEnd.Value);
        }

        // Filter by payment status
        if (request.PaymentStatus.HasValue)
        {
            var paymentStatus = (PaymentStatus)request.PaymentStatus.Value;
            if (paymentStatus == PaymentStatus.NotPaidYet)
            {
                predicate = predicate.And(x => x.PaymentId == null);
            }
            else
            {
                predicate = predicate.And(x => x.Payment != null && x.Payment.Status == paymentStatus);
            }
        }

        var query = repository.Get(predicate)
            .Include(t => t.Driver)
            .Include(t => t.Passenger)
            .Include(t => t.Payment)
            .Include(t => t.Feedbacks)
            .OrderByDescending(t => t.StartedAt)
            .Select(t => new DriverTripDto
            {
                TripId = t.Id,
                RiderName = $"{t.Passenger.FirstName} {t.Passenger.LastName}",
                Rating = t.Driver.Rating,
                PickUpTime = t.StartedAt,
                TripCost = t.Fare,
                Currency = t.Currency,
                PaymentMethod = t.PaymentMethodType.HasValue ? t.PaymentMethodType.Value.ToString() : "Unknown",
                PaymentStatus = t.Payment != null ? t.Payment.Status.ToString() : PaymentStatus.NotPaidYet.ToString(),
                TripStatus = t.Status.ToString(),
                TripRate = t.Feedbacks.FirstOrDefault(f => f.FromUserId == t.PassengerId) != null 
                    ? (double?)t.Feedbacks.FirstOrDefault(f => f.FromUserId == t.PassengerId)!.Rating 
                    : null,
                Comment = t.Feedbacks.FirstOrDefault(f => f.FromUserId == t.PassengerId) != null 
                    ? t.Feedbacks.FirstOrDefault(f => f.FromUserId == t.PassengerId)!.Comment 
                    : null
            });

        var result = await PagingHelper.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
        return result;
    }
}
