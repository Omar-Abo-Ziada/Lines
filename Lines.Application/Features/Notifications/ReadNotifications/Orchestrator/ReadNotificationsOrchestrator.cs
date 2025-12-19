using Lines.Application.Features.Notifications.GetNotifications.DTOs;
using Lines.Application.Features.Notifications.GetNotifications.Queries;
using Lines.Application.Features.Notifications.ReadNotifications.DTOs;
using Lines.Application.Features.Notifications.ReadNotifications.Queries;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Notifications.ReadNotifications.Orchestrator
{
    public record ReadNotificationsOrchestrator(Guid Id) : IRequest<Result<ReadNotificationsDTO>>;

    public class ReadNotificationsOrchestratorHandler : RequestHandlerBase<ReadNotificationsOrchestrator, Result<ReadNotificationsDTO>>
    {
        public ReadNotificationsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<ReadNotificationsDTO>> Handle(ReadNotificationsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ReadNotificationsQuery(request.Id ), cancellationToken).ConfigureAwait(false);
            return Result<ReadNotificationsDTO>.Success(result);
        }
    }
   
}
