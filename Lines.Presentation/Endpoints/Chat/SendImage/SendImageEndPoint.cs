using Lines.Application.Features.Chat;
using Lines.Application.Features.Chat.SendImage.Commands;
using Lines.Application.Features.Common.Commands;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Chat.SendImage
{
    public class SendImageEndPoint : BaseController<SendImageRequest, bool>
    {
        public SendImageEndPoint(BaseControllerParams<SendImageRequest> dependencyCollection) : base(dependencyCollection)
        {
        }

        [HttpPost("Chat/SendImage")]
        public async Task<ApiResponse<bool>> SendImage([FromForm] SendImageRequest request)
        {
            var validateResult = await ValidateRequestAsync(request);

            if (!validateResult.IsSuccess)
                return validateResult;

            var senderId = GetCurrentUserId();

            // Send message with image
            Domain.Shared.Result<bool> result = await _mediator.Send(new AddImageMessageOrchestrator(
                senderId,
                request.TripId, 
                request.Message, 
                request.Image));

            return HandleResult<bool, bool>(result);
        }
    }
}
