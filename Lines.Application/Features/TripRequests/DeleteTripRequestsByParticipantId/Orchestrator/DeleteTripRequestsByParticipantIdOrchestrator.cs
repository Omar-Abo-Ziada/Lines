using Lines.Application.Common;
using Lines.Application.Features.TripRequests.DeleteTripRequestById.Orchestrator;
using Lines.Application.Features.TripRequests.DeleteTripRequestsByParticipantId.Orchestrator;
using Lines.Application.Features.TripRequests.DeleteTripRequestsByUserId.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestsByUserId.Orchestrator
{
    public record DeleteTripRequestsByParticipantIdOrchestrator(Guid ParticipantId) : IRequest<Result<bool>>;

    public class DeleteTripRequestsByParticipantIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteTripRequestsByParticipantIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteTripRequestsByParticipantIdOrchestrator request, CancellationToken cancellationToken)
        {



            // get ids
           var tripRequestIds = await _mediator.Send(new GetTripRequestIdsByParticipantIdOrchestrator(request.ParticipantId), cancellationToken)
                                               .ConfigureAwait(false);



            
            foreach(var tripRequestId in tripRequestIds.Value)
            {
                await _mediator.Send(new DeleteTripRequestByIdOrchestrator(tripRequestId), cancellationToken)
                           .ConfigureAwait(false);
            }
            


            return Result<bool>.Success(true);
        }
    }
} 