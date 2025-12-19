using Lines.Application.Common;
using Lines.Application.Features.Chat.SendMessage.Queies;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Trips.GetTripById.Orchestrators;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Chat.SendMessage.Commands
{
    public record AddMessageOrchestrator(Guid SenderID, Guid TripID, string Content) : IRequest<Result<bool>>;

    public class AddMessageOrchestratorHandler : RequestHandlerBase<AddMessageOrchestrator, Result<bool>>
    {
        public AddMessageOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }

        public override async Task<Result<bool>> Handle(AddMessageOrchestrator request, CancellationToken cancellationToken)
        {

            var isEligible = await _mediator.Send(new IsEligibleToSendChatMessageQuery(
                request.SenderID, request.TripID, request.Content), cancellationToken)
                .ConfigureAwait(false);

            if (!isEligible)
            {
                return Result<bool>.Failure(ChatErrors.NonEligobleChattingPart("Invalid Sender"));
            }

            var trip = await _mediator.Send(new GetTripByIdOrchestrator(request.TripID));

            var recepientId = trip.Value.DriverId == request.SenderID ? trip.Value.PassengerId : trip.Value.DriverId;

            var messageID = await _mediator.Send(new AddMessageCommand(request.SenderID, recepientId, request.TripID, request.Content), cancellationToken)
                .ConfigureAwait(false);

            await _mediator.Send(new NotifyMessageAddedCommand(request.TripID, request.SenderID, recepientId, request.Content, messageID)
                , cancellationToken).ConfigureAwait(false);

            return Result<bool>.Success(true);
        }

    }
}
