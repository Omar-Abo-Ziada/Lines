using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Queries;
using Lines.Presentation.Common;
using Lines.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Lines.Application.Shared;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Steps;

public class GetRegistrationStepsEndpoint : BaseController<GetRegistrationStepsRequest, RegistrationStepsStatusDto>
{
    public GetRegistrationStepsEndpoint(BaseControllerParams<GetRegistrationStepsRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpGet("drivers/register/steps/{registrationToken}")]
    public async Task<ApiResponse<RegistrationStepsStatusDto>> HandleAsync(
        [FromRoute] string registrationToken,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetRegistrationStepsQuery(registrationToken);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegistrationStepsStatusDto>.ErrorResponse(result.Error, 404);
            }

            return ApiResponse<RegistrationStepsStatusDto>.SuccessResponse(result.Value);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegistrationStepsStatusDto>.ErrorResponse(
                new Error("General.Error", $"An error occurred: {ex.Message}", ErrorType.Failure),
                500);
        }
    }
}
