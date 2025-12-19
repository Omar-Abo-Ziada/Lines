
using Lines.Application.Features.Offers.GetOfferByRequestId.DTOs;
using Lines.Application.Features.Offers.GetOfferByRequestId.Queries;
using Lines.Application.Features.TripRequests.GetTripRequestById.Orchestrators;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;


namespace Lines.Application.Features.Offers.GetOfferByRequestId.Orchestrator
{
    public record GetOfferByRequestIdOrchestrator(Guid RequestId) : IRequest<Result<GetOfferByRequestIdResult>>;

    public class GetOfferByRequestIdOrchestratorHandler : RequestHandlerBase<GetOfferByRequestIdOrchestrator, Result<GetOfferByRequestIdResult>>
    {
        public GetOfferByRequestIdOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<GetOfferByRequestIdResult>> Handle(GetOfferByRequestIdOrchestrator request, CancellationToken cancellationToken)
        {
            var tripRequest = await _mediator.Send(new GetTripRequestByIdOrchestrator(request.RequestId));
            if(tripRequest.IsFailure)
            {
                return Result<GetOfferByRequestIdResult>.Failure(tripRequest.Error);
            }

            decimal? discountPercentage = null;
            decimal? maxValue = null;

            if (tripRequest.Value?.UserRewardId != null)
            {
                var userReward = await _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)tripRequest.Value.UserRewardId));
                if (userReward.IsFailure)
                {
                    return Result<GetOfferByRequestIdResult>.Failure(userReward.Error);
                }
                discountPercentage = userReward.Value?.Reward.DiscountPercentage;
                maxValue = userReward.Value?.Reward.MaxValue;
            }

            var result = await _mediator.Send(new GetOfferByRequestIdQuery(request.RequestId, discountPercentage, maxValue), cancellationToken).ConfigureAwait(false);
            return Result<GetOfferByRequestIdResult>.Success(result);
        }
    }
   
}
