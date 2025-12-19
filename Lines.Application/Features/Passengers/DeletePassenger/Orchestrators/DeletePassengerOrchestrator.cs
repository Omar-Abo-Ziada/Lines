using Lines.Application.Features.Activities.DeleteActivitiesByUserId.Orchestrator;
using Lines.Application.Features.EmergencyNumbers.DeleteEmergencyNumbersByUserId.Orchestrator;
using Lines.Application.Features.Feedback.DeleteFeedbackByUserId.Orchestrator;
using Lines.Application.Features.Messages.DeleteMessagesByUserId.Orchestrator;
using Lines.Application.Features.Notifications.DeleteNotificationsByUserId.Orchestrator;
using Lines.Application.Features.Passengers.DeletePassenger.Commands;
using Lines.Application.Features.Passengers.DeletePassenger.Queries;
using Lines.Application.Features.TripRequests.DeleteTripRequestsByUserId.Orchestrator;
using Lines.Application.Features.Trips.DeleteTripsByUserId.Orchestrator;
using Lines.Application.Interfaces;

namespace Lines.Presentation.Endpoints.Passengers
{
    public record DeletePassengerOrchestrator(Guid Id) : IRequest<Result<bool>>;
    public class DeletePassengerOrchestratorHandler(RequestHandlerBaseParameters parameters, IApplicationUserService applicationUserService) :
        RequestHandlerBase<DeletePassengerOrchestrator, Result<bool>>(parameters)
    {

        private readonly IApplicationUserService _applicationUserService = applicationUserService;

        public override async Task<Result<bool>> Handle(DeletePassengerOrchestrator request, CancellationToken cancellationToken)
        {
            var exists = await _mediator.Send(new CheckIfPassengerExistQuery(request.Id), cancellationToken)
                                        .ConfigureAwait(false);
            if (!exists)
            {
                return Result<bool>.Failure(Error.NullValue);
            }


            Guid userId = await _applicationUserService.GetUserIdByPassengerId(request.Id)
                                                        .ConfigureAwait(false);



            // add logic to delete all related entities if needed
            var isDeleted = await _applicationUserService.DeleteAsync(userId)
                                                      .ConfigureAwait(false); // Assuming DeleteByEmailAsync is a method to delete user by email

            ///TODO: delete user role 
            

            if (!isDeleted.Succeeded)
            {
                return Result<bool>.Failure(Error.NullValue);
            }

            var result = await _mediator.Send(new DeleteEmergencyNumbersByUserIdOrchestrator(userId), cancellationToken)
                           .ConfigureAwait(false);

            if(result.IsFailure)
            {
                return Result<bool>.Failure(Error.General);
            }

            result = await _mediator.Send(new DeleteActivitiesByUserIdOrchestrator(userId), cancellationToken)
                                    .ConfigureAwait(false);

             if (result.IsFailure)
             {
                 return Result<bool>.Failure(Error.General);
             }

             result = await _mediator.Send(new DeleteNotificationsByUserIdOrchestrator(userId), cancellationToken)
                                     .ConfigureAwait(false);

             if (result.IsFailure)
             {
                 return Result<bool>.Failure(Error.General);
             }

             result = await _mediator.Send(new DeleteFeedbackByUserIdOrchestrator(userId), cancellationToken)
                                     .ConfigureAwait(false);

             if (result.IsFailure)
             {
                 return Result<bool>.Failure(Error.General);
             }

             result = await _mediator.Send(new DeleteMessagesByUserIdOrchestrator(userId), cancellationToken)
                                     .ConfigureAwait(false);

             if (result.IsFailure)
             {
                 return Result<bool>.Failure(Error.General);
             }


            result = await _mediator.Send(new DeleteTripsByParticipantIdOrchestrator(request.Id), cancellationToken)
                                  .ConfigureAwait(false);

            if (result.IsFailure)
            {
                return Result<bool>.Failure(Error.General);
            }

            result = await _mediator.Send(new DeleteTripRequestsByParticipantIdOrchestrator(request.Id), cancellationToken)
                                     .ConfigureAwait(false);

             if (result.IsFailure)
             {
                 return Result<bool>.Failure(Error.General);
             }

            
             ///TODO: delete passenger payment method after sadek implement payment logic
             
            await _mediator.Send(new DeletePassengerCommand(request.Id), cancellationToken).ConfigureAwait(false);
            return Result<bool>.Success(true);
        }
    }
}