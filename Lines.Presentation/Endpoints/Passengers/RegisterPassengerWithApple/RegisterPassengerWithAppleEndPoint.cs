
using AspNet.Security.OAuth.Apple;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.Orchestrators;

using Lines.Application.Features.Users.Register_User.Orchestrators;
using Lines.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Lines.Presentation.Endpoints.Passengers.RegisterPassengerWithApple
{
    public class RegisterPassengerWithAppleEndPoint(BaseControllerParams<RegisterPassengerWithAppleRequest> dependencyCollection) : BaseController<RegisterPassengerWithAppleRequest, RegisterPassengerWithAppleResponse>(dependencyCollection)

    {

        [HttpGet("passenger/register-passenger-apple")]
        public IActionResult RegisterPassengerWithApple()
        {
    
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/api/passenger/register-passenger-apple-response";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, AppleAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("passenger/register-passenger-apple-response")]
        public async Task<ApiResponse<RegisterPassengerWithAppleResponse>> HandleRegisterPassengerWithApple(CancellationToken cancellationToken)
        {
           
            var appleRegisterResult = await _mediator.Send(new RegisterPassengerWithAppleOrchestrator(RegisterType.Passenger), cancellationToken);

            return HandleResult<RegisterPassengerWithAppleDTO, RegisterPassengerWithAppleResponse>(appleRegisterResult);
        }


    }


}
