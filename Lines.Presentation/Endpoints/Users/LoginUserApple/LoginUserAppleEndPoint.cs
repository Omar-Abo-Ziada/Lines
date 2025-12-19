using AspNet.Security.OAuth.Apple;
using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Features.Users.LoginUser.Orchestrators;
using Lines.Presentation.Endpoints.Users.LoginUser_Google;
using Microsoft.AspNetCore.Authentication;


namespace Lines.Presentation.Endpoints.Users.LoginUserApple
{
    public class LoginUserAppleEndPoint : BaseController<LoginUserAppleRequest, LoginUserAppleResponse>
    {
        private readonly BaseControllerParams<LoginUserAppleRequest> _dependencyCollection;

        public LoginUserAppleEndPoint(BaseControllerParams<LoginUserAppleRequest> dependencyCollection) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }


 
        [HttpGet("users/login-apple")]
        public IActionResult InitiateAppleLogin()
        {

            var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/passenger/register-passenger-apple-response";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, AppleAuthenticationDefaults.AuthenticationScheme);
        }
        [HttpGet("users/login-apple-response")]
        public async Task<ApiResponse<LoginUserAppleResponse>> HandleAppleResponse(CancellationToken cancellationToken)
        {
            var appleLoginResult =await  _mediator.Send(new LoginUserAppleOrchestrator() , cancellationToken);

            return HandleResult<LoginDTO, LoginUserAppleResponse>(appleLoginResult);
        }
    }
}
