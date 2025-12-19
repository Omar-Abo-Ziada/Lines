using Lines.Application.Features.Chat.SendMessage.Commands;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Chat.SendMessage
{
    public class SendMessageEndPoint : BaseController<SendMessageRequest, bool>
    {
        private readonly BaseControllerParams<SendMessageRequest> baseControllerParams;
        public SendMessageEndPoint(BaseControllerParams<SendMessageRequest> dependencyCollection, BaseControllerParams<SendMessageRequest> baseControllerParams) : base(dependencyCollection)
        {
            this.baseControllerParams = baseControllerParams;
        }

        [HttpPost("Chat/SendMessage")]
        public async Task<ApiResponse<bool>> Create([FromBody] SendMessageRequest request)
        {
            var ValidateResult = await ValidateRequestAsync(request);

            if (!ValidateResult.IsSuccess)
                return ValidateResult;

            var senderId = GetCurrentUserId();
            Result<bool> res = await _mediator.Send(new AddMessageOrchestrator(senderId ,request.TripId, request.Message));

            return HandleResult<bool, bool>(res);
        }
    }
}
