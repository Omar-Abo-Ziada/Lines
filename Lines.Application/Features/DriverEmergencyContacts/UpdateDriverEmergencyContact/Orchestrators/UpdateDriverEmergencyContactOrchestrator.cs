using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.Commands;
using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.DTOs;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.Orchestrators;

public record UpdateDriverEmergencyContactOrchestrator(Guid Id, Guid DriverId, string Name, string PhoneNumber) 
    : IRequest<Result<UpdateDriverEmergencyContactDto>>;

public class UpdateDriverEmergencyContactOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<DriverEmergencyContact> repository)
    : RequestHandlerBase<UpdateDriverEmergencyContactOrchestrator, Result<UpdateDriverEmergencyContactDto>>(parameters)
{
    public override async Task<Result<UpdateDriverEmergencyContactDto>> Handle(
        UpdateDriverEmergencyContactOrchestrator request,
        CancellationToken cancellationToken)
    {
        var emergencyContact = await repository.Get()
            .FirstOrDefaultAsync(dec => dec.Id == request.Id && !dec.IsDeleted, cancellationToken);

        if (emergencyContact == null)
        {
            return Result<UpdateDriverEmergencyContactDto>.Failure(
                new Error("NOT_FOUND", "Emergency contact not found", ErrorType.NotFound));
        }

        if (emergencyContact.DriverId != request.DriverId)
        {
            return Result<UpdateDriverEmergencyContactDto>.Failure(
                new Error("UNAUTHORIZED", "You can only update your own emergency contacts", ErrorType.UnAuthorized));
        }

        var result = await _mediator.Send(
            new UpdateDriverEmergencyContactCommand(
                request.Id,
                request.DriverId,
                request.Name,
                request.PhoneNumber,
                emergencyContact),
            cancellationToken).ConfigureAwait(false);

        return Result<UpdateDriverEmergencyContactDto>.Success(result);
    }
}

