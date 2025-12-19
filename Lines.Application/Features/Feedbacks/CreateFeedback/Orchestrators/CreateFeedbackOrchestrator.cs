using Lines.Application.Features.Drivers.GetDriverById.Orchestrators;
using Lines.Application.Features.Feedbacks.CreateFeedback.Commands;
using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.Trips.GetTripById.Orchestrators;

namespace Lines.Application.Features.Feedbacks.CreateFeedback.Orchestrators
{

    public record CreateFeedbackOrchestrator(
        Guid TripId,
        Guid FromUserId,
        string FromUserRole, 
        int Rating,
        string? Comment
    ) : IRequest<Result<Guid>>;  

    // Handler
    public class CreateFeedbackOrchestratorHandler(
        RequestHandlerBaseParameters parameters
    ) : RequestHandlerBase<CreateFeedbackOrchestrator, Result<Guid>>(parameters)
    {
        public override async Task<Result<Guid>> Handle(
            CreateFeedbackOrchestrator request,
            CancellationToken cancellationToken)
        {
           
            // update avg rating

            // 1- validate role first
            if (request.FromUserRole != "Passenger" && request.FromUserRole != "Driver")
            {
                return FeedbackErrors.SendFeedbackError("sender must be with role passenger or driver");
            }

            // 2- get trip by tripid 
            var trip = await _mediator.Send(new GetTripByIdOrchestrator(request.TripId), cancellationToken);
            if (trip is null)
                return Error.NullValue;

            var toUserId = Guid.Empty;

            // 3- get passenger or driver and update rating
            switch (request.FromUserRole)
            {
                case "Passenger":
                    {
                        toUserId = trip.Value.DriverId;

                        var driver = await _mediator.Send(new GetDriverByIdOrchestrator(trip.Value.DriverId));
                        driver.Value.UpdateRating(request.Rating);
                        break;
                    }
                case "Driver":
                    {
                        toUserId = trip.Value.PassengerId;

                        var passenger = await _mediator.Send(new GetPassengerByIdOrchestrator(trip.Value.PassengerId));
                        passenger.Value.UpdateRating(request.Rating);
                        break;
                    }
            }

            // Delegate to the actual Command                
            var result = await _mediator.Send(
                new CreateFeedbackCommand(
                    request.TripId,
                    request.FromUserId,
                    toUserId,
                    request.Rating,
                    request.Comment
                ),
                cancellationToken
            ).ConfigureAwait(false);


            return result;  
        }
    }
}
