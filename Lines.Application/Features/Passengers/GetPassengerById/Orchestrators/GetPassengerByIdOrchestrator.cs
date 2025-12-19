using Lines.Application.Features.Passengers.GetPassengerById.Queries;
using Lines.Application.Features.Passengers.SharedDtos;
using Lines.Domain.Models.Passengers;

namespace Lines.Application.Features.Passengers.GetPassengerById.Orchestrators
{
    public record GetPassengerByIdOrchestrator(Guid Id) : IRequest<Result<Passenger>>;

    public class GetPassengerByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetPassengerByIdOrchestrator, Result<Passenger>>(parameters)
    {
        public async override Task<Result<Passenger>> Handle(GetPassengerByIdOrchestrator request,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPassengerByIdQuery(request.Id), cancellationToken)
                                        .ConfigureAwait(false);
            
            return result == null ? Error.NullValue : result;
        }
    }
}
