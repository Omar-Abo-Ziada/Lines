using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.Notifications.GetNotifications.Queries;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Notifications.GetNotifications.Orchestrator
{
    public record GetNotificationsOrchestrator(Guid UserId, bool? IsRead) : IRequest<Result<List<GetNotificationsDTO>>>;

    public class GetNotificationsOrchestratorHandler : RequestHandlerBase<GetNotificationsOrchestrator, Result<List<GetNotificationsDTO>>>
    {
        public GetNotificationsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<List<GetNotificationsDTO>>> Handle(GetNotificationsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetNotificationsQuery(request.UserId ,request.IsRead), cancellationToken).ConfigureAwait(false);
            return Result<List<GetNotificationsDTO>>.Success(result);
        }
    }
   
}
