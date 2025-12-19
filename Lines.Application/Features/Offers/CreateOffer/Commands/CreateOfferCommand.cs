using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Offers.CreateOffer.Commands;
public record CreateOfferCommand(Guid driverId, decimal price, float distanceToArriveInMeters, int timeToArriveInMinutes, Guid tripRequestId) : IRequest<Result<Offer>>;

public class CreateOfferCommandHandler(RequestHandlerBaseParameters parameters, IRepository<Offer> offersRepository, IRepository<TripRequest> tripRequestsRepository,
    IRepository<Driver> driversRepository)
    : RequestHandlerBase<CreateOfferCommand, Result<Offer>>(parameters)
{
    private readonly IRepository<Offer> _offerRepository = offersRepository;
    private readonly IRepository<TripRequest> _tripRequestsRepository = tripRequestsRepository;
    private readonly IRepository<Driver> _driversRepository = driversRepository;

    public override async Task<Result<Offer>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        // 1 - validate the request (e.g., check if the driverId and tripRequestId are valid)
        if (request.driverId == Guid.Empty || request.tripRequestId == Guid.Empty)
        {
            return Result<Offer>.Failure(new Error(Code: "VALIDATION_ERROR",
                Description: "DriverId and TripRequestId are required.",
                Type: ErrorType.NotFound));
        }

        // 2- check if driver with this Id exists
        var driverId = await _driversRepository.SelectWhere(d => d.Id == request.driverId, selector: d => d.Id).FirstOrDefaultAsync(cancellationToken);
        if (driverId == Guid.Empty)
        {
            return Result<Offer>.Failure(new Error(Code: "DRIVER:NOTFOUND",
                   Description: "Driver with the specified Id does not exist.",
                   Type: ErrorType.NotFound));
        }

        // 3- check the trip request exists
        var tripRequest = await _tripRequestsRepository.SelectWhere(t => t.Id == request.tripRequestId, selector: t => new { t.Id, t.Status }).FirstOrDefaultAsync(cancellationToken);
        if (tripRequest is not null && tripRequest.Id == Guid.Empty)
        {
            return Result<Offer>.Failure(new Error(Code: "TRIPREQUEST:NOTFOUND",
                 Description: "Trip request with the specified Id does not exist.",
                 Type: ErrorType.NotFound));
        }

        // 4 - check for existing offers from the same driver for the same trip request
        var existingOffers = await _offerRepository.SelectWhere(o => o.DriverId == request.driverId && o.TripRequestId == request.tripRequestId, selector: o => o.Id).ToListAsync(cancellationToken);
        if (existingOffers.Any())
        {
            return Result<Offer>.Failure(new Error(Code: "OFFER:ALREADYEXISTS",
                Description: "Driver already has an offer for this trip request.",
                Type: ErrorType.NotFound));
        }

        // 5 - validate price, distance, and time (basic validation)
        if (request.price <= 0 || request.distanceToArriveInMeters < 0 || request.timeToArriveInMinutes < 0)
        {
            throw new ArgumentOutOfRangeException("Price must be greater than zero, and distance/time must be non-negative.");
        }

        // 6 - if the request is found and status is not pending return bad request
        if (tripRequest!.Status != TripRequestStatus.Pending)
            return Result<Offer>.Failure(new Error(Code: "TRIPREQUEST:NOTPENDING",
             Description: "Trip request is not pending.",
             Type: ErrorType.NotFound));


        // 7 - create the offer and attach it to the trip request
        var offer = new Offer(driverId: request.driverId, price: request.price, distanceToArriveInMeters: request.distanceToArriveInMeters, timeToArriveInMinutes: request.timeToArriveInMinutes, tripRequestId: request.tripRequestId);

        var result = await _offerRepository.AddAsync(offer, cancellationToken);
        return Result<Offer>.Success(result);
    }
}