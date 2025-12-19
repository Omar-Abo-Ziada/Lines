using Lines.Application.Features.Drivers.UpdateDriverAddress.Orchestrators;
using Lines.Application.Features.Drivers.UpdateDriverAddress.DTOs;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.UpdateDriverAddress;

[Authorize]
public class UpdateDriverAddressEndpoint : BaseController<UpdateDriverAddressRequest, UpdateDriverAddressResponse>
{
    public UpdateDriverAddressEndpoint(BaseControllerParams<UpdateDriverAddressRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpPut("drivers/address")]
    public async Task<ApiResponse<UpdateDriverAddressResponse>> UpdateAddress(UpdateDriverAddressRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<UpdateDriverAddressResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<UpdateDriverAddressResponse>.ErrorResponse(
                    new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
            }

            var addressInfoDto = new UpdateDriverAddressDto
            {
                Address = request.Address,
                CityId = request.CityId,
                SectorId = request.SectorId,
                PostalCode = request.PostalCode
            };

            var result = await _mediator.Send(new UpdateDriverAddressOrchestrator(userId, addressInfoDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<UpdateDriverAddressResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new UpdateDriverAddressResponse
            {
                Success = true,
                Message = "Address updated successfully."
            };

            return ApiResponse<UpdateDriverAddressResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<UpdateDriverAddressResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("500", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 500);
        }
    }
}
