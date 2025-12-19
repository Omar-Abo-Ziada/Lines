using Lines.Application.Features.Users.GetUserIdByMail.Orchestrators;
using Lines.Application.Features.Users.LoginUser.Commands;
using Lines.Application.Features.Users.LoginUser.DTOs;

namespace Lines.Application.Features.Users.LoginUser.Orchestrators;

public record LoginUserGoogleOrchestrator() : IRequest<Result<LoginDTO>>;

public class LoginUserGoogleOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<LoginUserGoogleOrchestrator, Result<LoginDTO>>(parameters)
{
    public override async Task<Result<LoginDTO>> Handle(LoginUserGoogleOrchestrator request, CancellationToken cancellationToken)
    {
        //1- Authenticate with Google and extract email
        var authenticateResult = await _mediator.Send(new AuthenticateGoogleCommand(), cancellationToken);
        if (authenticateResult.IsFailure)
        {
            return Result<LoginDTO>.Failure(authenticateResult.Error);
        }

        var email = authenticateResult.Value.Email;

        // 2- Check if user exists by email
        var existingUserResult = await _mediator.Send(new GetUserIdByMailOrchestrator(email), cancellationToken);

        if (existingUserResult.IsFailure)
        {
            return Result<LoginDTO>.Failure(UserErrors.LoginUserError($"No User Found with this Email : {email} , Please Register First"));
        }

        // 3- User exists, generate login token
        var loginResult = await _mediator.Send(new LoginGoogleCommand(email), cancellationToken);

        if (String.IsNullOrEmpty(loginResult.Token) || String.IsNullOrEmpty(loginResult.Role))
        {
            return Result<LoginDTO>.Failure(UserErrors.LoginUserError("User exists but login failed. Please confirm your email or use regular login."));
        }

        // 4- Return login token if success
        return Result<LoginDTO>.Success(new LoginDTO(loginResult.Token, loginResult.Role));
    }
}
