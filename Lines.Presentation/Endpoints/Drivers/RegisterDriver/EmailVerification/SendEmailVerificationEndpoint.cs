using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.EmailVerification;

public class SendEmailVerificationEndpoint : BaseController<SendEmailVerificationRequest, SendEmailVerificationResponse>
{
    public SendEmailVerificationEndpoint(BaseControllerParams<SendEmailVerificationRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/email/send-verification")]
    public async Task<ApiResponse<SendEmailVerificationResponse>> HandleAsync(
        SendEmailVerificationRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<SendEmailVerificationResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Send verification email
            var result = await _mediator.Send(new SendEmailVerificationOrchestrator(request.RegistrationToken), cancellationToken);

            if (!result.IsSuccess)
            {
                return ApiResponse<SendEmailVerificationResponse>.ErrorResponse(result.Error!, 400);
            }

            var response = new SendEmailVerificationResponse
            {
                IsSuccess = result.Value.IsSuccess,
                Message = result.Value.Message,
                IsEmailVerified = result.Value.IsEmailVerified
            };

            return ApiResponse<SendEmailVerificationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<SendEmailVerificationResponse>.ErrorResponse(
               new Error("INTERNAL_ERROR", ex.Message, ErrorType.Failure), 500);
        }
    }
}
