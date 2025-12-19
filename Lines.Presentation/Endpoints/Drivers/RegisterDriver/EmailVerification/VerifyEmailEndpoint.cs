using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class VerifyEmailEndpoint : BaseController<VerifyEmailRequest, VerifyEmailResponse>
{
    public VerifyEmailEndpoint(BaseControllerParams<VerifyEmailRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/email/verify")]
    public async Task<ApiResponse<VerifyEmailResponse>> HandleAsync(
        VerifyEmailRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<VerifyEmailResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Verify email
            var result = await _mediator.Send(new VerifyEmailOrchestrator(request.RegistrationToken, request.VerificationCode), cancellationToken);

            if (!result.IsSuccess)
            {
                return ApiResponse<VerifyEmailResponse>.ErrorResponse(result.Error!, 400);
            }

            var response = new VerifyEmailResponse
            {
                IsSuccess = result.Value.IsSuccess,
                Message = result.Value.Message,
                IsEmailVerified = result.Value.IsEmailVerified
            };

            return ApiResponse<VerifyEmailResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<VerifyEmailResponse>.ErrorResponse(
               new Error("INTERNAL_ERROR", ex.Message, ErrorType.Failure), 500);
        }
    }
}
