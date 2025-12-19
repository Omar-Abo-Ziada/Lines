using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.Orchestrator;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.Queries;
using Lines.Application.Features.TripRequests.DeleteTripRequestsByParticipantId.Queries;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestsByParticipantId.Orchestrator
{
    public record GetTripRequestIdsByParticipantIdOrchestrator(Guid ParticipantId) : IRequest<Result<List<Guid>>>;
   
    
    
    public class GetTripRequestIdsByParticipantIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetTripRequestIdsByParticipantIdOrchestrator, Result<List<Guid>>>(parameters)
    {
        public override async Task<Result<List<Guid>>> Handle(GetTripRequestIdsByParticipantIdOrchestrator request, CancellationToken cancellationToken)
        {
            var tripRequestIds = await _mediator.Send(new GetTripRequestIdsByParticipantIdQuery(request.ParticipantId), cancellationToken)
                                            .ConfigureAwait(false);



            return Result<List<Guid>>.Success(tripRequestIds);
        }
    }
}
