using Lines.Application.Features.EmergencyNumbers.GetEmergencyNumberById.Orchestrators;
using Lines.Application.Features.EmergencyNumbers.Shared;

namespace Lines.Application.Features.EmergencyNumbers;

public record EditEmergencyNumberOrchestrator(Guid Id,string Name, string PhoneNumber, EmergencyNumberType EmergencyNumberType,Guid? UserId) : IRequest<Result<EditEmergencyNumberDto>>;
public class EditEmergencyNumberOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<EditEmergencyNumberOrchestrator, Result<EditEmergencyNumberDto>>(parameters)
{
    public override async Task<Result<EditEmergencyNumberDto>> Handle(EditEmergencyNumberOrchestrator request, CancellationToken cancellationToken)
    {
        var emergencyNumberFromDb = await _mediator.Send(new GetEmergencyNumberByIdOrchestrator(request.Id));
        if (emergencyNumberFromDb.IsFailure)
        {
            return Result<EditEmergencyNumberDto>.Failure(emergencyNumberFromDb.Error);
        }
        // here >> do not get record twice from db
        if(request.UserId != emergencyNumberFromDb.Value.UserId)
        {
            return Result<EditEmergencyNumberDto>.Failure(EmergencyNumberErrors.UserMismatch);
        }

        var result = await _mediator.Send(new EditEmergencyNumberCommand(request.Id,request.Name,request.PhoneNumber,
                                          request.EmergencyNumberType, emergencyNumberFromDb.Value),cancellationToken).ConfigureAwait(false);
        return Result<EditEmergencyNumberDto>.Success(result);
    }
}