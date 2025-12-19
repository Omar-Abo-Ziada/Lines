using Lines.Application.Features.Passengers;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Passengers;

public class RegisterPassengerEndpoint : BaseController<RegisterPassengerRequest, Guid>
{
    private BaseControllerParams<RegisterPassengerRequest> _dependencyCollection;
    public RegisterPassengerEndpoint(BaseControllerParams<RegisterPassengerRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpPost("passengers/register")]
    public async Task<ApiResponse<Guid>> HandleAsync(RegisterPassengerRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var result = await _mediator.Send(new RegisterPassengerOrchestrator(request.FirstName, request.LastName, request.Email,
            request.PhoneNumber, request.Password, request.ReferralCode), cancellationToken);

        return result.IsSuccess ?
            ApiResponse<Guid>.SuccessResponse(result.Value, 200) :
            ApiResponse<Guid>.ErrorResponse(result.Error, 400);



    }
} 