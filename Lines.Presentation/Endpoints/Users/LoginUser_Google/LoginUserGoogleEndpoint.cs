using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Features.Users.LoginUser.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Users.LoginUser_Google;

public class LoginUserGoogleEndpoint : BaseController<LoginUserGoogleRequest, LoginUserGoogleResponse>
{
    private readonly BaseControllerParams<LoginUserGoogleRequest> _dependencyCollection;

    public LoginUserGoogleEndpoint(BaseControllerParams<LoginUserGoogleRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    //commented untill getting Google account from hisham
    [HttpGet("users/login-google")]
    public IActionResult InitiateGoogleLogin()
    {
        // Use the exact redirect URI that matches Google Console configuration
        var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/users/login-google-response";
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("users/login-google-response")]
    public async Task<ApiResponse<LoginUserGoogleResponse>> HandleGoogleResponse(CancellationToken cancellationToken)
    {

        // Call the orchestrator to handle all the business logic
        var googleLoginResult = await _mediator.Send(new LoginUserGoogleOrchestrator(), cancellationToken);

        return HandleResult<LoginDTO, LoginUserGoogleResponse>(googleLoginResult);
    }
}
