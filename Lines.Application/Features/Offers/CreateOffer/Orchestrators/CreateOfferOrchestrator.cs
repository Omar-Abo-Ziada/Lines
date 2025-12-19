using Lines.Application.Features.Offers.CreateOffer.Commands;
using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.Offers.CreateOffer.Notifications;
using Lines.Application.Features.Offers.CreateOffer.Queries.GetTripRequestById;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;
using Lines.Application.Helpers;

namespace Lines.Application.Features.Offers.CreateOffer.Orchestrators;
public record CreateOfferOrchestrator(decimal price,
    float distanceToArriveInMeters,
    int timeToArriveInMinutes,
    Guid tripRequestId,
    Guid userId) : IRequest<Result<OfferDTO>>;

public class CreateOfferOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
       : RequestHandlerBase<CreateOfferOrchestrator, Result<OfferDTO>>(parameters)
{
    private readonly IApplicationUserService _applicationUserService = applicationUserService;

    public override async Task<Result<OfferDTO>> Handle(CreateOfferOrchestrator request, CancellationToken cancellationToken)
    {
        // Validation : get the vechile of the driver and compare the KPPrice of his vehicle with the current estimated price => the estimated price shouldn't be greater than 30% of the vehile price
        // - get the user and ensure he is a driver include(driver.VechileId)
        // - get the vehcile of him and get the KmPrice of this vechile
        // - get the trip request and and get the distance
        // - compare ( offer.price ) must <= 1.3 (tripRequest.Distance X Vechile.KmPrice)

        var vehileInfoDTO = _applicationUserService.GetVechileKmPriceForUser(request.userId);

        if (vehileInfoDTO?.DriverId == Guid.Empty)
        {
            return Result<OfferDTO>.Failure(new Error(Code: "OFFER.USERNOTDRIVER:NOTFOUND",
                Description: "The user is not found or not a driver",
                Type: ErrorType.NotFound));

        }
        if (vehileInfoDTO?.KmPrice is null)
        {
            return Result<OfferDTO>.Failure(new Error(Code: "OFFER.DRIVE_VECHILE:NOTFOUND",
                Description: "The driver doesnt have a vechile or vechile doesnt have KmPrice",
                Type: ErrorType.NotFound));
        }


        // 2 - get the trip request By Id and include the offers
        var tripRequestResult = await _mediator.Send(new GetTripRequestByIdQuery(request.tripRequestId), cancellationToken);

        // 3 - if the request is not found return not found
        if (tripRequestResult.IsFailure)
            return Result<OfferDTO>.Failure(new Error(Code: tripRequestResult.Error.Code
              , Description: tripRequestResult.Error.Description,
              Type: tripRequestResult.Error.Type));

        if ((decimal)request.price > ((decimal)1.3 * (decimal)tripRequestResult.Value.Distance * vehileInfoDTO.KmPrice))
        {
            return Result<OfferDTO>.Failure(new Error(Code: "OFFER.PRICE_HIGHERTHAN_30%KMPRICE",
                Description: "Price can't be More than 30% of vechile Km-Price",
                Type: ErrorType.Validation));
        }

        // 4 - create the offer and attach it to the trip request
        var offerResult = await _mediator.Send(new CreateOfferCommand(vehileInfoDTO.DriverId, request.price, request.distanceToArriveInMeters, request.timeToArriveInMinutes, request.tripRequestId));
        if (offerResult.IsFailure)
            return Result<OfferDTO>.Failure(new Error(Code: offerResult.Error.Code
              , Description: offerResult.Error.Description,
              Type: offerResult.Error.Type));

        decimal? fareAfterRewardApplied = null;

        // check if that offer for trip request has reward
        if (tripRequestResult.Value.UserRewardId is not null)
        {
            var userReward = await _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)tripRequestResult.Value.UserRewardId));
            if(userReward.IsFailure)
            {
                return Result<OfferDTO>.Failure(Error.NullValue);
            }

           fareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(offerResult.Value.Price,
                                                 userReward.Value.Reward.DiscountPercentage, userReward.Value.Reward.MaxValue);
        }

        // 5- Notify the passenger of the new offer
        var passengerId = tripRequestResult.Value.PassengerId;
        var offer = offerResult.Value;
        await _mediator.Send(new NotifyNewOfferCreatedCommand(
            PassengerId: passengerId,
            TripRequestId: request.tripRequestId,
            OfferId: offer.Id,
            DriverId: vehileInfoDTO.DriverId,
            Price: offer.Price,
            DistanceToArriveInMeters: offer.DistanceToArriveInMeters,
            TimeToArriveInMinutes: offer.TimeToArriveInMinutes,
            FareAfterRewardApplied: fareAfterRewardApplied
        ), cancellationToken);

        return Result<OfferDTO>.Success(offer.Adapt<OfferDTO>());
    }
}