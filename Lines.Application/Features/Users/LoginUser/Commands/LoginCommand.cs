using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Users.LoginUser.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginDTO>;

public class LoginCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<LoginCommand, LoginDTO>(parameters)
{
    private readonly IApplicationUserService _authService = authService;

    public override async Task<LoginDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);
        return result;
    }
}