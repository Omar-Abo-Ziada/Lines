using Lines.Application.Features.Users.UpdatePassword.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.UpdatePassword
{
    public class UpdatePasswordEndpoint : BaseController<UpdatePasswordRequest, bool>
    {
        private readonly BaseControllerParams<UpdatePasswordRequest> _dependencyCollection;

        public UpdatePasswordEndpoint(BaseControllerParams<UpdatePasswordRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPut("users/update-password")]
        public async Task<ApiResponse<bool>> HandleAsync(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                new UpdatePasswordOrchestrator(request.UserId, request.NewPassword, request.CurrentPassword),
                cancellationToken);

            return result.IsSuccess
                ? ApiResponse<bool>.SuccessResponse(true, 200)
                : ApiResponse<bool>.ErrorResponse(result.Error, 400);
        }
    }
}
