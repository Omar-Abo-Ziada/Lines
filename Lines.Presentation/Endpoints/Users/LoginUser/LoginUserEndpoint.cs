using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Features.Users.LoginUser.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.LoginPassenger
{
    public class LoginUserEndpoint : BaseController<LoginUserRequest, LoginUserResponse>
    {
        private readonly BaseControllerParams<LoginUserRequest> _dependencyCollection;

        public LoginUserEndpoint(BaseControllerParams<LoginUserRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("users/login")]
        public async Task<ApiResponse<LoginUserResponse>> HandleAsync(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(
                new LoginUserOrchestrator(request.Email, request.Password),
                cancellationToken
            );

            return result.IsSuccess ?
                HandleResult<LoginDTO, LoginUserResponse>(result) :
                ApiResponse<LoginUserResponse>.ErrorResponse(result.Error, 400);
        }
    }
}
