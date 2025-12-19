using Lines.Application.Common;
using Lines.Application.DTOs;
using Lines.Application.Features.Chat.GetMessages.Queries;
using Lines.Application.Features.Chat.MarkMessagesAsRead.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Chat.GetMessages.Orchestrator
{
    public record GetMessagesOrchestrator(Guid TripId, Guid UserId, int PageNumber = 1, int PageSize = 50, bool MarkAsRead = true) : IRequest<Result<PagingDto<MessageDto>>>;

    public class GetMessagesOrchestratorHandler : RequestHandlerBase<GetMessagesOrchestrator, Result<PagingDto<MessageDto>>>
    {
        public GetMessagesOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }

        public override async Task<Result<PagingDto<MessageDto>>> Handle(GetMessagesOrchestrator request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await Validate(request);
            
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // Get messages for the trip
            var getMessagesQuery = new GetMessagesQuery(request.TripId, request.UserId, request.PageNumber, request.PageSize);
            var messagesResult = await _mediator.Send(getMessagesQuery, cancellationToken)
                .ConfigureAwait(false);

            if (!messagesResult.IsSuccess)
            {
                return Result<PagingDto<MessageDto>>.Failure(messagesResult.Error);
            }

            // Mark messages as read if requested
            if (request.MarkAsRead)
            {
                var markAsReadCommand = new MarkMessagesAsReadCommand(request.TripId, request.UserId);
                await _mediator.Send(markAsReadCommand, cancellationToken)
                    .ConfigureAwait(false);
            }

            return Result<PagingDto<MessageDto>>.Success(messagesResult.Value);
        }

        private async Task<Result<PagingDto<MessageDto>>> Validate(GetMessagesOrchestrator request)
        {
            if (request.TripId == Guid.Empty)
            {
                return Result<PagingDto<MessageDto>>.Failure(ChatErrors.InvalidTripId("Trip ID cannot be empty"));
            }

            if (request.UserId == Guid.Empty)
            {
                return Result<PagingDto<MessageDto>>.Failure(ChatErrors.InvalidUserId("User ID cannot be empty"));
            }

            if (request.PageNumber < 1)
            {
                return Result<PagingDto<MessageDto>>.Failure(ChatErrors.InvalidPageNumber("Page number must be greater than 0"));
            }

            if (request.PageSize < 1 || request.PageSize > 100)
            {
                return Result<PagingDto<MessageDto>>.Failure(ChatErrors.InvalidPageSize("Page size must be between 1 and 100"));
            }

            return Result<PagingDto<MessageDto>>.Success(default);
        }
    }
}
