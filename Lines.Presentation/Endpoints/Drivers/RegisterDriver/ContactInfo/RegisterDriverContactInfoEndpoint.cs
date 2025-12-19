using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.ContactInfo;

public class RegisterDriverContactInfoEndpoint : BaseController<RegisterDriverContactInfoRequest, RegisterDriverContactInfoResponse>
{
    public RegisterDriverContactInfoEndpoint(BaseControllerParams<RegisterDriverContactInfoRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step2/contact-info")]
    public async Task<ApiResponse<RegisterDriverContactInfoResponse>> HandleAsync(
        RegisterDriverContactInfoRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverContactInfoResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var contactInfoDto = new RegisterDriverContactInfoDto(
                request.Email
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverContactInfoOrchestrator(request.RegistrationToken, contactInfoDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverContactInfoResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverContactInfoResponse
            {
                Success = true,
                Message = "Contact information saved successfully. Please proceed to address information.",
                CurrentStep = 2
            };

            return ApiResponse<RegisterDriverContactInfoResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverContactInfoResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
