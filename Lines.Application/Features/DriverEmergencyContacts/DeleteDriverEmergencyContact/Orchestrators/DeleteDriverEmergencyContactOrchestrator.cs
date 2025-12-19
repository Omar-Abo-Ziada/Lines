using Lines.Application.Features.DriverEmergencyContacts.DeleteDriverEmergencyContact.Commands;
using Lines.Application.Shared;

namespace Lines.Application.Features.DriverEmergencyContacts.DeleteDriverEmergencyContact.Orchestrators;

public record DeleteDriverEmergencyContactOrchestrator(Guid Id, Guid DriverId) 
    : IRequest<Result<bool>>;

public class DeleteDriverEmergencyContactOrchestratorHandler(
    RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<DeleteDriverEmergencyContactOrchestrator, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(
        DeleteDriverEmergencyContactOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteDriverEmergencyContactCommand(request.Id, request.DriverId),
            cancellationToken).ConfigureAwait(false);

        return result;
    }
}

