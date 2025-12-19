using Lines.Application.Features.Users.LoginUser.Commands;
using Lines.Application.Features.Users.LoginUser.DTOs;

namespace Lines.Application.Features.Users.LoginUser.Orchestrators;

public record LoginUserOrchestrator(string email, string password) : IRequest<Result<LoginDTO>>;

public class LoginPassengerOrchestratorHandler(RequestHandlerBaseParameters parameters)
    : RequestHandlerBase<LoginUserOrchestrator, Result<LoginDTO>>(parameters)
{
    public override async Task<Result<LoginDTO>> Handle(LoginUserOrchestrator request, CancellationToken cancellationToken)
    {
        // Delegate to the LoginCommand
        var result = await _mediator.Send(new LoginCommand(request.email, request.password), cancellationToken);

        if (!string.IsNullOrEmpty(result.Token))
        {
            return Result<LoginDTO>.Success(new LoginDTO(result.Token, result.Role));
        }

        return Result<LoginDTO>.Failure(UserErrors.LoginUserError("Invalid credentials or unconfirmed email"));
    }
}
