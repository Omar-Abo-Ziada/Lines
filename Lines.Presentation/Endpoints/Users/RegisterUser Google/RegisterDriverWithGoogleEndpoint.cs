using Lines.Application.Features.Users.Register_User.DTOs;
using Lines.Application.Features.Users.Register_User.Orchestrators;
using Lines.Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace Lines.Presentation.Endpoints.Users.RegisterUser_Google;

public class RegisterDriverWithGoogleEndpoint(BaseControllerParams<RegisterUserGoogleRequests> dependencyCollection) : BaseController<RegisterUserGoogleRequests, RegisterUserGoogleResponse>(dependencyCollection)
{

    [HttpGet("users/register-driver-google")]
    public IActionResult RegisterPassengerWithGoogle()
    {
        // Use the exact redirect URI that matches Google Console configuration
        var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/users/register-driver-google-response";
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("users/register-driver-google-response")]
    public async Task<ApiResponse<RegisterUserGoogleResponse>> HandleRegisterPassengerWithGoogle(CancellationToken cancellationToken)
    {
        // Call the orchestrator to handle all the business logic
        var googleRegisterResult = await _mediator.Send(new RegisterUserWithGoogleOrchestrator(RegisterType.Driver), cancellationToken);

        return HandleResult<RegisterUserDTO, RegisterUserGoogleResponse>(googleRegisterResult);
    }
}
