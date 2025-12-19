using Lines.Application.Features.Users.Register_User_Google.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;

namespace Lines.Application.Features.Users.LoginUser.Commands;

public record AuthenticateGoogleCommand() : IRequest<Result<UserGoogleClaims>>;

public class AuthenticateGoogleCommandHandler(RequestHandlerBaseParameters parameters, IHttpContextAccessor httpContextAccessor)
    : RequestHandlerBase<AuthenticateGoogleCommand, Result<UserGoogleClaims>>(parameters)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public override async Task<Result<UserGoogleClaims>> Handle(AuthenticateGoogleCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return Result<UserGoogleClaims>.Failure(UserErrors.LoginUserError("HttpContext not available"));
        }

        // Authenticate with Google
        var result = await httpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return Result<UserGoogleClaims>.Failure(UserErrors.LoginUserError("Google authentication failed"));
        }

        // Extract claims
        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        if (claims == null)
        {
            return Result<UserGoogleClaims>.Failure(UserErrors.LoginUserError("No claims found"));
        }

        // Extract email from claims
        var userInfo = claims.ToDictionary(c => c.Type, c => c.Value);
        var email = userInfo.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") ?? string.Empty;
        var googleProviderKey = userInfo.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") ?? string.Empty;
        var firstName = userInfo.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname") ?? string.Empty;
        var lastName = userInfo.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") ?? string.Empty;

        if (string.IsNullOrEmpty(email))
        {
            return Result<UserGoogleClaims>.Failure(UserErrors.LoginUserError("Email not found in Google claims"));
        }

        var userGoogleClaims = new UserGoogleClaims(firstName, lastName, email, googleProviderKey);

        return Result<UserGoogleClaims>.Success(userGoogleClaims);
    }
}