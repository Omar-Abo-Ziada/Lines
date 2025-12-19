using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Apple;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Users.LoginUser.Commands
{



    public record AuthenticateAppleCommand() : IRequest<Result<PassengerAppleClaims>>;

    public class AuthenticateAppleCommandHandler(
        RequestHandlerBaseParameters parameters,
        IHttpContextAccessor httpContextAccessor)
        : RequestHandlerBase<AuthenticateAppleCommand, Result<PassengerAppleClaims>>(parameters)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public override async Task<Result<PassengerAppleClaims>> Handle(AuthenticateAppleCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Result<PassengerAppleClaims>.Failure(UserErrors.LoginUserError("HttpContext not available"));
            }

            // Authenticate with Apple
            var result = await httpContext.AuthenticateAsync(AppleAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Result<PassengerAppleClaims>.Failure(UserErrors.LoginUserError("Apple authentication failed"));
            }

            // Extract claims
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            if (claims == null)
            {
                return Result<PassengerAppleClaims>.Failure(UserErrors.LoginUserError("No claims found"));
            }

            // Extract user info
            var userInfo = claims.ToDictionary(c => c.Type, c => c.Value);

            var email = userInfo.GetValueOrDefault("email") ?? string.Empty;
            var firstName = userInfo.GetValueOrDefault("given_name") ?? string.Empty;
            var lastName = userInfo.GetValueOrDefault("family_name") ?? string.Empty;
            var appleProviderKey = userInfo.GetValueOrDefault("sub") ?? string.Empty; // unique Apple ID

            if (string.IsNullOrEmpty(email))
            {
                return Result<PassengerAppleClaims>.Failure(UserErrors.LoginUserError("Email not found in Apple claims"));
            }

            var userAppleClaims = new PassengerAppleClaims(firstName, lastName, email, appleProviderKey);

            return Result<PassengerAppleClaims>.Success(userAppleClaims);
        }
    }
}
