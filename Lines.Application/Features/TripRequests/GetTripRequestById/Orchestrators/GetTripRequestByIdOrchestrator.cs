using Lines.Application.Features.TripRequests.GetTripRequestById.Queries;

namespace Lines.Application.Features.TripRequests.GetTripRequestById.Orchestrators
{
    public record GetTripRequestByIdOrchestrator(Guid Id) : IRequest<Result<TripRequest?>>;

    public class GetTripRequestByIdOrchestratorHandler(RequestHandlerBaseParameters parameters) :
        RequestHandlerBase<GetTripRequestByIdOrchestrator, Result<TripRequest?>>(parameters)
    {
        public override async Task<Result<TripRequest?>> Handle(GetTripRequestByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var tripRequest = await _mediator.Send(new GetTripRequestByIdQuery(request.Id), cancellationToken);

            return tripRequest == null ? Result<TripRequest?>.Failure(Error.NullValue) :
                                        Result<TripRequest?>.Success(tripRequest);
        }
    }
}
