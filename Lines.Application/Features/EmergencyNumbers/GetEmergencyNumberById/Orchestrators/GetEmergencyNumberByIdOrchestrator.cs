using Lines.Application.Features.EmergencyNumbers.GetEmergencyNumberById.Queries;
using Lines.Domain.Models.Users;

namespace Lines.Application.Features.EmergencyNumbers.GetEmergencyNumberById.Orchestrators
{
    public record GetEmergencyNumberByIdOrchestrator(Guid Id) : IRequest<Result<EmergencyNumber>>;


    public class GetEmergencyNumberByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetEmergencyNumberByIdOrchestrator, Result<EmergencyNumber>>(parameters)
    {
        public override async Task<Result<EmergencyNumber>> Handle(GetEmergencyNumberByIdOrchestrator request, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetEmergencyNumberByIdQuery(request.Id), cancellationToken)
                                        .ConfigureAwait(false);

            return result == null? Result<EmergencyNumber>.Failure(Error.NullValue)
                                 : Result<EmergencyNumber>.Success(result);
        }
    }
}
