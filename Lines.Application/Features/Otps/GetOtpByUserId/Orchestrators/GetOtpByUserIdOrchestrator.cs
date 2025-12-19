using Lines.Application.Common;
using Lines.Application.Features.Otps.GetOtpByUserId.Queries;
using Lines.Domain.Models.User;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Otps.GetOtpByUserId.Orchestrators
{
    public record GetOtpByUserIdOrchestrator(Guid USerId) : IRequest<Result<Otp?>>;

    public class GetOtpByUserIdOrchestratorHandler(
                      RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<GetOtpByUserIdOrchestrator, Result<Otp?>>(parameters)
    {
        public override async Task<Result<Otp?>> Handle(GetOtpByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            var otp = await _mediator.Send(new GetOtpByUserIdQuery(request.USerId), cancellationToken).ConfigureAwait(false);

            return Result<Otp?>.Success(otp);
        }
    }
}
