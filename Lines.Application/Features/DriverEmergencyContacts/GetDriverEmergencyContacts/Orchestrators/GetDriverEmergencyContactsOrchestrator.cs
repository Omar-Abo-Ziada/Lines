using Lines.Application.Features.DriverEmergencyContacts.GetDriverEmergencyContacts.Queries;
using Lines.Application.Features.DriverEmergencyContacts.Shared.DTOs;
using Lines.Application.Shared;

namespace Lines.Application.Features.DriverEmergencyContacts.GetDriverEmergencyContacts.Orchestrators;

public record GetDriverEmergencyContactsOrchestrator(Guid DriverId) 
    : IRequest<Result<List<DriverEmergencyContactDto>>>;

public class GetDriverEmergencyContactsOrchestratorHandler(
    RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<GetDriverEmergencyContactsOrchestrator, Result<List<DriverEmergencyContactDto>>>(parameters)
{
    public override async Task<Result<List<DriverEmergencyContactDto>>> Handle(
        GetDriverEmergencyContactsOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetDriverEmergencyContactsQuery(request.DriverId),
            cancellationToken).ConfigureAwait(false);
        
        return Result<List<DriverEmergencyContactDto>>.Success(result);
    }
}

