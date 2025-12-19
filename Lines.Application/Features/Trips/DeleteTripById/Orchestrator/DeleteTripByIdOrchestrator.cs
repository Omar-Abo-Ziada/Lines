using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesByTripId.Orchestrator;
using Lines.Application.Features.Trips.DeleteTripById.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Trips.DeleteTripById.Orchestrator
{
    public record DeleteTripByIdOrchestrator(Guid TripId) : IRequest<Result<bool>>;

    public class DeleteTripByIdOrchestratorHandler(
      RequestHandlerBaseParameters parameters)
      : RequestHandlerBase<DeleteTripByIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteTripByIdOrchestrator request, CancellationToken cancellationToken)
        {
            // Call the orchestrators that deletes the related data 

            // delete messages by trip id
            await _mediator.Send(new DeleteMessagesByTripIdOrchestrator(request.TripId), cancellationToken);

            ///TODO: after sadek payment logic completed
            // await _mediator.Send(new DeleteTripPaymentsOrchestrator(request.TripId), cancellationToken);



            // delete the trip itself
            var result = await _mediator.Send(new DeleteTripByIdCommand(request.TripId), cancellationToken);

            return result == true
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(Error.NullValue);
        }
    }


}
