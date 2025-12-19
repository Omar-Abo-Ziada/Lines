using Lines.Application.Features.EmergencyNumbers;
using Lines.Domain.Enums;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public class CreateEmergencyNumberEndpoint : BaseController<CreateEmergencyNumberRequest, CreateEmergencyNumberResponse>
{
    private readonly BaseControllerParams<CreateEmergencyNumberRequest> _dependencyCollection;
    public CreateEmergencyNumberEndpoint(BaseControllerParams<CreateEmergencyNumberRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpPost("emergencynumber/create")]
    public async Task<ApiResponse<CreateEmergencyNumberResponse>> HandleAsync([FromBody] CreateEmergencyNumberRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var userId = GetCurrentUserId();

        var result = await _mediator.Send(new CreateEmergencyNumberOrchestrator(request.Name, request.PhoneNumber,
                                          EmergencyNumberType.PersonalEmergencyNumber, userId), cancellationToken).ConfigureAwait(false);
        return HandleResult<CreateEmergencyNumberDto,CreateEmergencyNumberResponse>(result);
    }
}