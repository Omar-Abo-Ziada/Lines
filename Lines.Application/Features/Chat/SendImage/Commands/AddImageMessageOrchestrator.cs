using Lines.Application.Features.Chat.SendMessage.Commands;
using Lines.Application.Features.Chat.SendMessage.Queies;
using Lines.Application.Features.Common.Commands;
using Lines.Application.Features.Trips.GetTripById.Orchestrators;
using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Chat.SendImage.Commands
{
    public record AddImageMessageOrchestrator(Guid SenderID, Guid TripID, string Content, IFormFile Image) : IRequest<Result<bool>>;

    public class AddImageMessageOrchestratorHandler : RequestHandlerBase<AddImageMessageOrchestrator, Result<bool>>
    {
        public AddImageMessageOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }

        public override async Task<Result<bool>> Handle(AddImageMessageOrchestrator request, CancellationToken cancellationToken)
        {
            var isEligible = await _mediator.Send(new IsEligibleToSendChatMessageQuery(
              request.SenderID, request.TripID, request.Content), cancellationToken)
              .ConfigureAwait(false);

            if (!isEligible)
            {
                return Result<bool>.Failure(ChatErrors.NonEligobleChattingPart("Invalid Sender"));
            }

            Domain.Shared.Result<string> uploadResult = await _mediator.Send(new UploadImageOrchestrator(request.Image));

            if (!uploadResult.IsSuccess)
            {
                return Result<bool>.Failure(uploadResult.Error);
            }

            var trip = await _mediator.Send(new GetTripByIdOrchestrator(request.TripID));

            var recipientId = trip.Value.DriverId == request.SenderID ? trip.Value.PassengerId : trip.Value.DriverId;

            var imagePath = uploadResult.Value;

            var messageID = await _mediator.Send(new AddImageMessageCommand(request.SenderID, recipientId, request.TripID, request.Content, imagePath), cancellationToken)
                .ConfigureAwait(false);

            await _mediator.Send(new NotifyMessageAddedCommand(request.TripID, request.SenderID, recipientId, request.Content, messageID, imagePath)
                , cancellationToken).ConfigureAwait(false);

            return Result<bool>.Success(true);
        }
    }
}
