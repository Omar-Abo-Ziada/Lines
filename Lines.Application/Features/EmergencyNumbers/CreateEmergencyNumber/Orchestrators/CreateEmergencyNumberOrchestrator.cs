using Lines.Application.Features.EmergencyNumbers.Shared;

namespace Lines.Application.Features.EmergencyNumbers;

public record CreateEmergencyNumberOrchestrator(string Name, string PhoneNumber, EmergencyNumberType EmergencyNumberType, Guid? UserId) : IRequest<Result<CreateEmergencyNumberDto>>;
public class CreateEmergencyNumberOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<CreateEmergencyNumberOrchestrator, Result<CreateEmergencyNumberDto>>(parameters)
{

    public override async Task<Result<CreateEmergencyNumberDto>> Handle(CreateEmergencyNumberOrchestrator request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new CreateEmergencyNumberCommand(request.Name,request.PhoneNumber,request.EmergencyNumberType,request.UserId),cancellationToken).ConfigureAwait(false);
        return Result<CreateEmergencyNumberDto>.Success(result);
    }

    private async Task<Result<CreateEmergencyNumberDto>> ValidateRequest(CreateEmergencyNumberOrchestrator request)
    {
        var isFound = await _mediator.Send(new CheckIfEmergencyNumberExist(request.PhoneNumber, request.EmergencyNumberType, request.UserId)).ConfigureAwait(false);
        if (isFound)
        {
            return Result<CreateEmergencyNumberDto>.Failure(EmergencyNumberErrors.EmergencyNumberExist);
        }
        return Result<CreateEmergencyNumberDto>.Success(null!);
    }
}