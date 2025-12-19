using Lines.Application.Features.Trips.GetAllTripsByPassengerId.DTOs;
using Lines.Application.Features.Trips.GetAllTripsByPassengerId.Queries;

namespace Lines.Application.Features.Trips.GetAllTripsByPassengerId.Orchestrators
{
    public record GetAllTripsByPassengerIdOrchestrator(Guid passengerId, Guid userId) : IRequest<Result<List<GetAllTripsByPassengerIdDto>>>;


    public class GetAllTripsByPassengerIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetAllTripsByPassengerIdOrchestrator, Result<List<GetAllTripsByPassengerIdDto>>>(parameters)
    {
        public async override Task<Result<List<GetAllTripsByPassengerIdDto>>> Handle(
                       GetAllTripsByPassengerIdOrchestrator request,
                                  CancellationToken cancellationToken)
        {
            var tripsDto = await _mediator.Send(new GetAllTripsByPassengerIdQuery(request.passengerId, request.userId));

            
            return Result<List<GetAllTripsByPassengerIdDto>>.Success(tripsDto);
        }
    }
}
