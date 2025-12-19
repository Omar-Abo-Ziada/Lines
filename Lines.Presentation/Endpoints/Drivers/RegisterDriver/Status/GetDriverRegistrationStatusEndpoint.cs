using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Queries;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Status;

public class GetDriverRegistrationStatusEndpoint : BaseController<GetDriverRegistrationStatusRequest, DriverRegistrationStatusDto>
{
    public GetDriverRegistrationStatusEndpoint(BaseControllerParams<GetDriverRegistrationStatusRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpGet("drivers/register/status")]
    public async Task<ApiResponse<DriverRegistrationStatusDto>> HandleAsync(
        [FromQuery] GetDriverRegistrationStatusRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Call query
            var result = await _mediator.Send(new GetDriverRegistrationStatusQuery(request.RegistrationToken), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<DriverRegistrationStatusDto>.ErrorResponse(result.Error, 400);
            }

            return ApiResponse<DriverRegistrationStatusDto>.SuccessResponse(result.Value);
        }
        catch (Exception ex)
        {
            return ApiResponse<DriverRegistrationStatusDto>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
