using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Users.LoginUser.DTOs;

namespace Lines.Application.Features.Users.LoginUser.Commands
{
    public record LoginAppleCommand(string Email) : IRequest<LoginDTO>;

    public class LoginAppleCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<LoginAppleCommand, LoginDTO>(parameters)
    {
        private readonly IApplicationUserService _authService = authService;

        public override Task<LoginDTO> Handle(LoginAppleCommand request, CancellationToken cancellationToken)
        {
            // For Apple users, we need to handle authentication differently since they don't have a password
            var result = _authService.LoginAppleAsync(request.Email);

            return result;

        }
    }


}
