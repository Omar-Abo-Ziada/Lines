using Lines.Application.Common;
using Lines.Application.Features.Otps.UpdateOtp.Commands;
using Lines.Application.Shared;
using Lines.Domain.Models.User;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.UpdateOtp.Orchestrators
{
    public record UpdateOtpOrchestrator(Otp otp) : IRequest<Result>;

    public class UpdateOtpOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<UpdateOtpOrchestrator, Result>(parameters)
    {
        public override async Task<Result> Handle(UpdateOtpOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.otp is null)
            {
                return Result.Failure(OtpErrors.InvalidOtpError("OTP cannot be null"));
            }

            var result = await _mediator.Send(new UpdateOtpCommand(request.otp), cancellationToken);
            if (!result)
            {
                return Result.Failure(Error.General);
            }

            return Result.Success();
        }
    }
}
