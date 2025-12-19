using Lines.Application.Features.Users.ResendEmailVerificationOtp.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.ResendEmailVerificationOtp;

public class ResendEmailVerificationOtpEndpoint : BaseController<ResendEmailVerificationOtpRequest, ResendEmailVerificationOtpResponse>
{
    public ResendEmailVerificationOtpEndpoint(BaseControllerParams<ResendEmailVerificationOtpRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpPost("users/resend-email-verification-otp")]
    public async Task<ApiResponse<ResendEmailVerificationOtpResponse>> ResendOtp(ResendEmailVerificationOtpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<ResendEmailVerificationOtpResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Call orchestrator
            var result = await _mediator.Send(new ResendEmailVerificationOtpOrchestrator(request.Email), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<ResendEmailVerificationOtpResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new ResendEmailVerificationOtpResponse
            {
                Success = true,
                Message = "If the email exists and is not verified, a verification OTP has been sent."
            };

            return ApiResponse<ResendEmailVerificationOtpResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<ResendEmailVerificationOtpResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("500", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 500);
        }
    }
}
