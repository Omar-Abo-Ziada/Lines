using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.DTOs;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.Queries;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Messages.DeleteMessagesByTripId.Orchestrator
{
    public record GetMessageIdsAndTypesByTripIdOrchestrator(Guid TripId) : IRequest<Result<List<GetMessageIdsAndTypesByTripIdDto>>>;

    public class GetMessageIdsAndTypesByTripIdOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : RequestHandlerBase<GetMessageIdsAndTypesByTripIdOrchestrator, Result<List<GetMessageIdsAndTypesByTripIdDto>>>(parameters)
    {
        public override async Task<Result<List<GetMessageIdsAndTypesByTripIdDto>>> Handle(GetMessageIdsAndTypesByTripIdOrchestrator request, CancellationToken cancellationToken)
        {
            var messages = await _mediator.Send(new GetMessageIdsAndTypesByTripIdQuery(request.TripId), cancellationToken)
                                            .ConfigureAwait(false);


            return Result<List<GetMessageIdsAndTypesByTripIdDto>>.Success(messages);
        }
    }
}