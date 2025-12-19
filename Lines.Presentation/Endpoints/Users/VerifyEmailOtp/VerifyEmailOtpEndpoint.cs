using Lines.Application.Features.Users.VerifyEmailOtp.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.VerifyEmailOtp;

public class VerifyEmailOtpEndpoint : BaseController<VerifyEmailOtpRequest, VerifyEmailOtpResponse>
{
    public VerifyEmailOtpEndpoint(BaseControllerParams<VerifyEmailOtpRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpPost("users/verify-email-otp")]
    public async Task<ApiResponse<VerifyEmailOtpResponse>> VerifyOtp(VerifyEmailOtpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<VerifyEmailOtpResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Call orchestrator
            var result = await _mediator.Send(new VerifyEmailOtpOrchestrator(request.Email, request.Otp), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<VerifyEmailOtpResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new VerifyEmailOtpResponse
            {
                Success = true,
                Message = "Email verified successfully.",
                EmailVerified = true
            };

            return ApiResponse<VerifyEmailOtpResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<VerifyEmailOtpResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("500", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 500);
        }
    }
}
