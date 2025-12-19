using Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs;
using Lines.Application.Features.Users.GetUserById.Queries;

namespace Lines.Application.Features.Users.GetUserById.Orchestrators
{
    public record GetPassengerAndDriverIdsByUserIdOrchestrator(Guid UserId) : IRequest<Result<GetPassengerAndDriverIdsDTO>>;

    public class GetPassengerAndDriverIdsByUserIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetPassengerAndDriverIdsByUserIdOrchestrator, Result<GetPassengerAndDriverIdsDTO>>(parameters)
    {
        public async override Task<Result<GetPassengerAndDriverIdsDTO>> Handle(
            GetPassengerAndDriverIdsByUserIdOrchestrator request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetPassengerAndDriverIdsByUserIdQuery(request.UserId),
                cancellationToken
            ).ConfigureAwait(false);

            if (result is null)
            {
                return Error.NullValue;
            }

            return result;
        }
    }
}
