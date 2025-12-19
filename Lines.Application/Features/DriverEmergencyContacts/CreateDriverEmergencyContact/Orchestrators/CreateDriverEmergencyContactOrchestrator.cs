using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.Commands;
using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.DTOs;
using Lines.Application.Shared;

namespace Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.Orchestrators;

public record CreateDriverEmergencyContactOrchestrator(Guid DriverId, string Name, string PhoneNumber) 
    : IRequest<Result<CreateDriverEmergencyContactDto>>;

public class CreateDriverEmergencyContactOrchestratorHandler(
    RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<CreateDriverEmergencyContactOrchestrator, Result<CreateDriverEmergencyContactDto>>(parameters)
{
    public override async Task<Result<CreateDriverEmergencyContactDto>> Handle(
        CreateDriverEmergencyContactOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateDriverEmergencyContactCommand(request.DriverId, request.Name, request.PhoneNumber),
            cancellationToken).ConfigureAwait(false);
        
        return Result<CreateDriverEmergencyContactDto>.Success(result);
    }
}

