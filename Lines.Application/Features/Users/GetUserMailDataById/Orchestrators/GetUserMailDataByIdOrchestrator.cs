using Lines.Application.Common;
using Lines.Application.Features.Users.GetUserMailDataById.DTOs;
using Lines.Application.Features.Users.GetUserMailDataById.Queries;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Users.GetUserById.Orchestrators
{
    public record GetUserMailDataByIdOrchestrator(Guid userId) : IRequest<Result<UserMailDataDto?>>;

    public class GetUserMailDataByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetUserMailDataByIdOrchestrator, Result<UserMailDataDto?>>(parameters)
    {
        public override async Task<Result<UserMailDataDto?>> Handle(GetUserMailDataByIdOrchestrator request , CancellationToken cancellationToken)
        {
            var mailData = await _mediator.Send(new GetUserMailDataByIdQuery(request.userId));

            if (mailData is null)
            {
                return Result<UserMailDataDto?>.Failure(Error.NullValue);
            }

            return Result<UserMailDataDto?>.Success(mailData);
        }
    }
}
