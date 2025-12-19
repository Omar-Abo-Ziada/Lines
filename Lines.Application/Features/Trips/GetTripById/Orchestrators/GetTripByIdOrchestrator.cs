using Lines.Application.Features.Trips.GetTripById.Queries;

namespace Lines.Application.Features.Trips.GetTripById.Orchestrators
{
    public record GetTripByIdOrchestrator(Guid TripId) : IRequest<Result<Trip>>;

    public class GetTripByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetTripByIdOrchestrator, Result<Trip>>(parameters)
    {
        public async override Task<Result<Trip>> Handle(
                       GetTripByIdOrchestrator request,
                                  CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetTripByIdQuery(request.TripId),cancellationToken)
                                        .ConfigureAwait(false);

            return result == null ? Error.NullValue : result;
        }
    }
}
