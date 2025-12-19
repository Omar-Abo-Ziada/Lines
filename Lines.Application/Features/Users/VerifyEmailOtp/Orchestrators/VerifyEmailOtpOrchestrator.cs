using Lines.Application.Features.Otps.ValidateOtpByUserId.Orchestrators;
using Lines.Application.Features.Otps.UpdateOtp.Orchestrators;
using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Users.VerifyEmailOtp.Orchestrators;

public record VerifyEmailOtpOrchestrator(string email, string otp) : IRequest<Result<bool>>;

public class VerifyEmailOtpOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService)
    : RequestHandlerBase<VerifyEmailOtpOrchestrator, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(VerifyEmailOtpOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user ID by email
            var userIdResult = await _mediator.Send(new GetUserIdByMailOrchestrator(request.email), cancellationToken);
            if (userIdResult.IsFailure || userIdResult.Value == null)
            {
                return Result<bool>.Failure(new Application.Shared.Error("400", "User not found.", Application.Shared.ErrorType.NotFound));
            }

            // Validate OTP
            var validateResult = await _mediator.Send(new ValidateOtpByUserIdOrchestrator(userIdResult.Value.Value, request.otp), cancellationToken);
            if (validateResult.IsFailure)
            {
                return Result<bool>.Failure(validateResult.Error);
            }

            if (!validateResult.Value.IsValid)
            {
                return Result<bool>.Failure(new Application.Shared.Error("400", "Invalid or expired OTP.", Application.Shared.ErrorType.Validation));
            }

            // Confirm email
            var confirmResult = await applicationUserService.ConfirmMailAsync(userIdResult.Value.Value);
            if (confirmResult.IsFailure)
            {
                return Result<bool>.Failure(confirmResult.Error);
            }

            // Mark OTP as used
            if (validateResult.Value.Otp != null)
            {
                validateResult.Value.Otp.MarkAsUsed();
                await _mediator.Send(new UpdateOtpOrchestrator(validateResult.Value.Otp), cancellationToken);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Application.Shared.Error("500", $"An error occurred while verifying email OTP: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}
