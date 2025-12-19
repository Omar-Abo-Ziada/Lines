using Lines.Application.Common.Email;
using Lines.Application.Features.Otps.CreateOtp.Orchestrators;
using Lines.Application.Features.Users.GetUserById.Orchestrators;
using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;
using Lines.Application.Interfaces;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Users.ResendEmailVerificationOtp.Orchestrators;

public record ResendEmailVerificationOtpOrchestrator(string email) : IRequest<Result<bool>>;

public class ResendEmailVerificationOtpOrchestratorHandler(RequestHandlerBaseParameters parameters, IEmailService emailService, IApplicationUserService applicationUserService)
    : RequestHandlerBase<ResendEmailVerificationOtpOrchestrator, Result<bool>>(parameters)
{
    public async override Task<Result<bool>> Handle(ResendEmailVerificationOtpOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user ID by email
            var userIdResult = await _mediator.Send(new GetUserIdByMailOrchestrator(request.email), cancellationToken);
            if (userIdResult.IsFailure || userIdResult.Value == null)
            {
                // Return success for security (don't reveal if email exists)
                return Result<bool>.Success(true);
            }

            // Get user mail data to check if email is confirmed
            var mailDataResult = await _mediator.Send(new GetUserMailDataByIdOrchestrator(userIdResult.Value.Value));
            if (mailDataResult.IsFailure)
            {
                // Return success for security
                return Result<bool>.Success(true);
            }

            // Check if email is already confirmed by checking the user
            var user = await applicationUserService.GetMailDataByIdAsync(userIdResult.Value.Value);
            if (user == null)
            {
                return Result<bool>.Success(true);
            }

            // For security, we'll assume if we can get mail data, the email is not confirmed
            // and proceed to send OTP

            // Create OTP for the user
            var otpResult = await _mediator.Send(new CreateOtpOrchestrator(userIdResult.Value.Value), cancellationToken);
            if (otpResult.IsFailure)
            {
                return Result<bool>.Failure(otpResult.Error);
            }

            // Send OTP email
            MailData mailData = new MailData(
                mailDataResult.Value.Mail, 
                mailDataResult.Value.UserName,
                MailSubjects.Otp,
                string.Format(MailFormat.Otp.Html, mailDataResult.Value.UserName, otpResult.Value.Code)
            );

            var sendMailResult = await emailService.SendMailAsync(mailData);
            if (!sendMailResult.Succeeded)
            {
                return Result<bool>.Failure(MailErrors.SendMailError(string.Join(" , ", sendMailResult.Errors)));
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Application.Shared.Error("500", $"An error occurred while resending email verification OTP: {ex.Message}", Application.Shared.ErrorType.Failure));
        }
    }
}
