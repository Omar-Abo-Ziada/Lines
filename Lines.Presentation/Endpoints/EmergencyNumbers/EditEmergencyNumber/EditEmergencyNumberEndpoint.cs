using Lines.Application.Features.EmergencyNumbers;
using Lines.Domain.Enums;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;
[AllowAnonymous]
public class EditEmergencyNumberEndpoint : BaseController<EditEmergencyNumberRequest, EditEmergencyNumberResponse>
{
    private readonly BaseControllerParams<EditEmergencyNumberRequest> _dependencyCollection;
    public EditEmergencyNumberEndpoint(BaseControllerParams<EditEmergencyNumberRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpPut("emergencynumber/Edit")]
    public async Task<ApiResponse<EditEmergencyNumberResponse>> HandleAsync([FromBody] EditEmergencyNumberRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var userId = GetCurrentUserId();

        var result = await _mediator.Send(new EditEmergencyNumberOrchestrator(request.Id, request.Name, request.PhoneNumber,
                                          EmergencyNumberType.PersonalEmergencyNumber ,userId), cancellationToken).ConfigureAwait(false);  // only personal emergency numbers can be edited by passenger
        return HandleResult<EditEmergencyNumberDto,EditEmergencyNumberResponse>(result);
    }
}