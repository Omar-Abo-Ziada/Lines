using Lines.Application.Features.TripRequests.Commands;
using Lines.Domain.Shared;

namespace Lines.Application.Features.TripRequests.EditTripRequestPrice.Orchestrator
{
    public record EditTripRequestPriceOrchestrator(Guid TripRequestId, decimal NewPrice, Guid UserId)
        : IRequest<Result>;

    public class EditTripRequestPriceOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<EditTripRequestPriceOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(EditTripRequestPriceOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new EditTripRequestPriceCommand(request.TripRequestId, request.NewPrice, request.UserId),
                cancellationToken);
            return result;
        }
    }
}
