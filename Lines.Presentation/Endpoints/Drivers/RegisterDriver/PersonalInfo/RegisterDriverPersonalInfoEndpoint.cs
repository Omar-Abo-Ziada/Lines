using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.PersonalInfo;

public class RegisterDriverPersonalInfoEndpoint : BaseController<RegisterDriverPersonalInfoRequest, RegisterDriverPersonalInfoResponse>
{
    public RegisterDriverPersonalInfoEndpoint(BaseControllerParams<RegisterDriverPersonalInfoRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step1/personal-info")]
    public async Task<ApiResponse<RegisterDriverPersonalInfoResponse>> HandleAsync(
        RegisterDriverPersonalInfoRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverPersonalInfoResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var personalInfoDto = new RegisterDriverPersonalInfoDto(
                request.PersonalPicture,
                request.FirstName,
                request.LastName,
                request.CompanyName,
                request.CommercialRegistration,
                request.DateOfBirth,
                request.PhoneNumber,
                request.Password,
                request.ConfirmPassword,
                request.IdentityType
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverPersonalInfoOrchestrator(personalInfoDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverPersonalInfoResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverPersonalInfoResponse
            {
                Success = true,
                Message = "Personal information saved successfully. Please proceed to contact information.",
                SessionToken = result.Value, // This is now the RegistrationToken
                CurrentStep = 1
            };

            return ApiResponse<RegisterDriverPersonalInfoResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverPersonalInfoResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
