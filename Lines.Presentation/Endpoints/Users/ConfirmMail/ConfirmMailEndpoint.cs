using Lines.Application.Features.Users.ConfirmMail.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.ConfirmMail
{
    public class ConfirmMailEndpoint : BaseController<ConfirmMailRequest, bool>
    {
        private readonly BaseControllerParams<ConfirmMailRequest> _dependencyCollection;

        public ConfirmMailEndpoint(BaseControllerParams<ConfirmMailRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("users/confirm-mail")]
        public async Task<ApiResponse<bool>> HandleAsync(ConfirmMailRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                          new ConfirmMailOrchestrator(request.UserId, request.Otp),
                          cancellationToken
                         );

            return result.IsSuccess ?
                HandleResult<bool, bool>(result.IsSuccess) :
                ApiResponse<bool>.ErrorResponse(result.Error, 400);
        }
    }
}
