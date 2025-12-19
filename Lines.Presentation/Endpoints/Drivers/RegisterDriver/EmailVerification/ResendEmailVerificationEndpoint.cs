using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class ResendEmailVerificationEndpoint : BaseController<ResendEmailVerificationRequest, ResendEmailVerificationResponse>
{
    public ResendEmailVerificationEndpoint(BaseControllerParams<ResendEmailVerificationRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/email/resend")]
    public async Task<ApiResponse<ResendEmailVerificationResponse>> HandleAsync(
        ResendEmailVerificationRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<ResendEmailVerificationResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Resend verification email
            var result = await _mediator.Send(new ResendEmailVerificationOrchestrator(request.RegistrationToken), cancellationToken);

            if (!result.IsSuccess)
            {
                return ApiResponse<ResendEmailVerificationResponse>.ErrorResponse(result.Error!, 400);
            }

            var response = new ResendEmailVerificationResponse
            {
                IsSuccess = result.Value.IsSuccess,
                Message = result.Value.Message,
                IsEmailVerified = result.Value.IsEmailVerified
            };

            return ApiResponse<ResendEmailVerificationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<ResendEmailVerificationResponse>.ErrorResponse(
                new Error("INTERNAL_ERROR", ex.Message, ErrorType.Failure), 500);
        }
    }
}
