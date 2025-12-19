using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Address;

public class RegisterDriverAddressEndpoint : BaseController<RegisterDriverAddressRequest, RegisterDriverAddressResponse>
{
    public RegisterDriverAddressEndpoint(BaseControllerParams<RegisterDriverAddressRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step3/address")]
    [Consumes("multipart/form-data")]
    public async Task<ApiResponse<RegisterDriverAddressResponse>> HandleAsync(
        RegisterDriverAddressRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverAddressResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var addressDto = new RegisterDriverAddressDto(
                request.CityId,
                request.SectorId,        // NEW - replaces Region
                request.Address,         // RENAMED from Street
                request.PostalCode,
                request.LimousineBadge   // NEW
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverAddressOrchestrator(request.RegistrationToken, addressDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverAddressResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverAddressResponse
            {
                Success = true,
                Message = "Address information saved successfully. Please proceed to license information.",
                CurrentStep = 3
            };

            return ApiResponse<RegisterDriverAddressResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverAddressResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
