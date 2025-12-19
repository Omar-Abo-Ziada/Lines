using Lines.Application.Common;
using Lines.Application.Features.Otps.CreateOtp.Commands;
using Lines.Application.Features.Otps.CreateOtp.DTOs;
using Lines.Application.Features.Otps.DeleteOtpByUserId.Orchestrators;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.CreateOtp.Orchestrators
{
    public record CreateOtpOrchestrator(Guid userId) : IRequest<Result<CreateOtpDto>>;

    public class CreateOtpOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<CreateOtpOrchestrator, Result<CreateOtpDto>>(parameters)
    {
        public override async Task<Result<CreateOtpDto>> Handle(CreateOtpOrchestrator request, CancellationToken cancellationToken)
        {

            var deletionResult = await _mediator.Send(new DeleteOtpsByUserIdOrchestrator(request.userId), cancellationToken);
            if (deletionResult.IsFailure)
            {
                return Result<CreateOtpDto>.Failure(deletionResult.Error);
            }

            var result = await _mediator.Send(new CreateOtpCommand(request.userId), cancellationToken)
                                        .ConfigureAwait(false);

            return Result<CreateOtpDto>.Success(result);
        }
    }

}
