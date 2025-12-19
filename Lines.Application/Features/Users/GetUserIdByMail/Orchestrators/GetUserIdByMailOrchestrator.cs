using Lines.Application.Features.Users.GetUserIdByMail.Queries;

namespace Lines.Application.Features.Users.GetUserIdByMail.Orchestrators
{
    public record GetUserIdByMailOrchestrator(string email) : IRequest<Result<Guid?>>;

    public class GetUserIdByMailOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : RequestHandlerBase<GetUserIdByMailOrchestrator , Result<Guid?>>(parameters)
    {
        public async override Task<Result<Guid?>> Handle(GetUserIdByMailOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserIdByMailQuery(request.email), cancellationToken)
                                        .ConfigureAwait(false);

            if (result is null)
            {
                return Result<Guid?>.Failure(Error.NullValue);
            }

            return Result<Guid?>.Success(result);
        }
    }
}
