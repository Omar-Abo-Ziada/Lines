using Lines.Application.Common;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Chat.MarkMessagesAsRead.Commands
{
    public record MarkMessagesAsReadOrchestrator(Guid TripId, Guid UserId) : IRequest<Result<bool>>;

    public class MarkMessagesAsReadOrchestratorHandler : RequestHandlerBase<MarkMessagesAsReadOrchestrator, Result<bool>>
    {
        public MarkMessagesAsReadOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }

        public override async Task<Result<bool>> Handle(MarkMessagesAsReadOrchestrator request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await Validate(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // Mark messages as read
            var markAsReadCommand = new MarkMessagesAsReadCommand(request.TripId, request.UserId);
            await _mediator.Send(markAsReadCommand, cancellationToken)
                .ConfigureAwait(false);

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>> Validate(MarkMessagesAsReadOrchestrator request)
        {
            if (request.TripId == Guid.Empty)
            {
                return Result<bool>.Failure(ChatErrors.InvalidTripId("Trip ID cannot be empty"));
            }

            if (request.UserId == Guid.Empty)
            {
                return Result<bool>.Failure(ChatErrors.InvalidUserId("User ID cannot be empty"));
            }

            return Result<bool>.Success(default);
        }
    }
}
