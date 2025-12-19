using Lines.Application.Features.Chat;
using Lines.Application.Features.Chat.MarkMessagesAsRead.Commands;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.Chat.GetMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Chat.MarkMessageAsRead
{
    [Authorize]
    public class MarkMessagesAsReadEndPoint : BaseController<MarkMessagesAsReadRequest, bool>
    {
        public MarkMessagesAsReadEndPoint(BaseControllerParams<MarkMessagesAsReadRequest> dependencyCollection) : base(dependencyCollection)
        {
        }

        [HttpPost("Chat/MarkMessagesAsRead")]
        public async Task<ApiResponse<bool>> MarkMessagesAsRead([FromBody] MarkMessagesAsReadRequest request)
        {
            var validateResult = await ValidateRequestAsync(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            // Get the current user ID from the authenticated user
            var userId = GetCurrentUserId();

            if (userId == Guid.Empty)
            {
                return HandleResult<bool, bool>(Result<bool>.Failure(ChatErrors.InvalidUserId("Un Authorized")));
            }

            var orchestrator = new MarkMessagesAsReadOrchestrator(request.TripId, userId);
            Result<bool> result = await _mediator.Send(orchestrator);

            return HandleResult<bool, bool>(result);
        }
    }
}
