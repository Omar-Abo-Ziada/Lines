using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.LoginUser.Commands;

public record LoginGoogleCommand(string Email) : IRequest<LoginDTO>;

public class LoginGoogleCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<LoginGoogleCommand, LoginDTO>(parameters)
{
    private readonly IApplicationUserService _authService = authService;

    public override async Task<LoginDTO> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
    {
        // For Google users, we need to handle authentication differently since they don't have a password
        var result = await _authService.LoginGoogleAsync(request.Email);
        return result;
    }
}