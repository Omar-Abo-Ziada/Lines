
//using Lines.Application.Features.Trips.PassengerStart.Commands;

//namespace Lines.Application.Features.Trips.PassengerStart.Orchestrators
//{
//    public record PassengerRequestStartOrchestrator(Guid TripId, Guid PassengerId)
//        : IRequest<Result<bool>>;

//    public class PassengerRequestStartOrchestratorHandler(RequestHandlerBaseParameters parameters)
//        : RequestHandlerBase<PassengerRequestStartOrchestrator, Result<bool>>(parameters)
//    {
//        public override async Task<Result<bool>> Handle(PassengerRequestStartOrchestrator request, CancellationToken cancellationToken)
//        {
//            var result = await _mediator.Send(
//                new PassengerRequestStartCommand(request.TripId, request.PassengerId),
//                cancellationToken);

//            return result;
//        }
//    }
//}

 
