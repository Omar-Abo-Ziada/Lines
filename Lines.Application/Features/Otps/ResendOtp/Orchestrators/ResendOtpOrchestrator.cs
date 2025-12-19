using Lines.Application.Common.Email;
using Lines.Application.Features.Otps.CreateOtp.Orchestrators;
using Lines.Application.Features.Users.GetUserById.Orchestrators;
using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;

namespace Lines.Application.Features.Otps.ResendOtp.Orchestrators
{
    public record ResendOtpOrchestrator (Guid? userId , string? email) : IRequest<Result<Guid>>;

    public class ResendOtpOrchestratorHandler(RequestHandlerBaseParameters parameters, IEmailService emailService)
        : RequestHandlerBase<ResendOtpOrchestrator, Result<Guid>>(parameters)
    {
        public override async Task<Result<Guid>> Handle(ResendOtpOrchestrator request, CancellationToken cancellationToken)
        {
            Guid userId = request.userId ?? Guid.Empty;

            if (request.userId is null)
            {
                // get user id by mail
               var result = await _mediator.Send(new GetUserIdByMailOrchestrator(request.email?? string.Empty));
                if (result.IsFailure)
                {
                    return Result<Guid>.Failure(result.Error);
                }
                userId = result.Value?? Guid.Empty;  
            }
            // Delegate to the CreateOtpOrchestrator
            var otpResult = await _mediator.Send(new CreateOtpOrchestrator(userId), cancellationToken);

            if (otpResult.IsFailure)
            {
                return Result<Guid>.Failure(otpResult.Error);
            }

            var mailDataResult = await _mediator.Send(new GetUserMailDataByIdOrchestrator(userId));

            if (mailDataResult.IsFailure)
            {
                return Result<Guid>.Failure(mailDataResult.Error);
            }

            MailData mailData = new MailData(mailDataResult?.Value?.Mail?? string.Empty, mailDataResult?.Value?.UserName?? string.Empty,
                                MailSubjects.Otp,string.Format(MailFormat.Otp.Html, mailDataResult?.Value?.UserName, otpResult.Value.Code));

            var sendMailResult = await emailService.SendMailAsync(mailData);

            if(!sendMailResult.Succeeded)
            {
                return Result<Guid>.Failure(MailErrors.SendMailError(string.Join(" , ", sendMailResult.Errors)));
            }

            return Result<Guid>.Success(userId);
        }
    }
}
