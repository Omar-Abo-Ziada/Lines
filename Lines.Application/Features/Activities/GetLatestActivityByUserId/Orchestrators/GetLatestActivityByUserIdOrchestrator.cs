using Lines.Application.Features.Activities.GetLatestActivityByUserId.Queries;
using Lines.Domain.Models.Users;

namespace Lines.Application.Features.Activities.GetLatestActivityByUserId.Orchestrators
{
    public record GetLatestActivityByUserIdOrchestrator(Guid userId) : IRequest<Result<Activity?>>;

    public class GetLatestActivityByUserIdOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Activity> _repository)
        : RequestHandlerBase<GetLatestActivityByUserIdOrchestrator, Result<Activity?>>(parameters)
    {
        public override async Task<Result<Activity?>> Handle(GetLatestActivityByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
           var activity = await _mediator.Send(new GetLatestActivityByUserIdQuery(request.userId));

            return Result<Activity?>.Success(activity);
                                   
        }
    }
}
