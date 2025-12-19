using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.License;

public class RegisterDriverLicenseEndpoint : BaseController<RegisterDriverLicenseRequest, RegisterDriverLicenseResponse>
{
    public RegisterDriverLicenseEndpoint(BaseControllerParams<RegisterDriverLicenseRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step4/license")]
    public async Task<ApiResponse<RegisterDriverLicenseResponse>> HandleAsync(
        RegisterDriverLicenseRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverLicenseResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var licenseDto = new RegisterDriverLicenseDto(
                request.LicenseNumber,
                request.ExpiryDate,
                request.LicenseImages
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverLicenseOrchestrator(request.RegistrationToken, licenseDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverLicenseResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverLicenseResponse
            {
                Success = true,
                Message = "License information saved successfully. Please proceed to vehicle information.",
                CurrentStep = 4
            };

            return ApiResponse<RegisterDriverLicenseResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverLicenseResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
