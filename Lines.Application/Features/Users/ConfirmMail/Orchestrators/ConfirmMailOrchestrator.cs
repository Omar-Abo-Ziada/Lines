using Lines.Application.Common;
using Lines.Application.Features.Otps.UpdateOtp.Orchestrators;
using Lines.Application.Features.Otps.ValidateOtpByUserId.Orchestrators;
using Lines.Application.Features.Users.ConfirmMail.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Users.ConfirmMail.Orchestrators
{
    public record ConfirmMailOrchestrator(Guid userId , string otp) : IRequest<Result>;


        public class ConfirmMailOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<ConfirmMailOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(ConfirmMailOrchestrator request, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new ValidateOtpByUserIdOrchestrator(request.userId, request.otp));

            if (result.IsSuccess)
            {
                var (isValid, otp) = result.Value;

                if(isValid)
                {
                    otp?.MarkAsUsed();
                    await _mediator.Send(new UpdateOtpOrchestrator(otp));

                    return await _mediator.Send(new ConfirmMailCommand(request.userId));
                }
                return Result.Failure(new Error(string.Empty, "Invalid Otp or it has been expired", ErrorType.Validation));
            }
            return Result.Failure(result.Error);
        }
    }
}
