using Lines.Application.Features.Chat.GetMessages.Orchestrator;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Chat.GetMessages
{
    [Authorize]
    public class GetMessagesEndPoint : BaseController<GetMessagesRequest, GetMessagesResponse>
    {
        public GetMessagesEndPoint(BaseControllerParams<GetMessagesRequest> dependencyCollection) : base(dependencyCollection)
        {
        }

        [HttpGet("Chat/GetMessages")]
        public async Task<ApiResponse<GetMessagesResponse>> GetMessages([FromQuery] GetMessagesRequest request)
        {
            var validateResult = await ValidateRequestAsync(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            // Get the current user ID from the authenticated user
            var userId = GetCurrentUserId();

            if (userId == Guid.Empty)
            {
                return HandleResult<GetMessagesResponse, GetMessagesResponse>
                    (Domain.Shared.Result<GetMessagesResponse>.Failure(Error.General));
            }

            var orchestrator = new GetMessagesOrchestrator(request.TripId, userId, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(orchestrator);

            if (!result.IsSuccess)
            {
                return HandleResult<GetMessagesResponse, GetMessagesResponse>(result.Error);
            }

            var response = new GetMessagesResponse
            {
                Messages = result.Value
            };

            return HandleResult<GetMessagesResponse, GetMessagesResponse>(response);
        }
    }
}
