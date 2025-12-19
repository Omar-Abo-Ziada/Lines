using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lines.Application.Features.Passengers.RegisterPassengerWithApple.DTOs;
using Lines.Application.Features.Users.Register_User.DTOs;
using Lines.Application.Features.Users.Register_User_Google.Commands;

namespace Lines.Application.Features.Passengers.RegisterPassengerWithApple.Commands
{
    public record RegisterPassengerWithAppleCommand(string FirstName, string LastName, string Email, RegisterType RegisterType, string AppleProviderKey,
    string referralCode, int Points) : IRequest<Result<RegisterPassengerWithAppleDTO>>;

    public class RegisterPassengerWithAppleCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<RegisterPassengerWithAppleCommand, Result<RegisterPassengerWithAppleDTO>>(parameters)
    {
        private readonly IApplicationUserService _authService = authService;
        public override async Task<Result<RegisterPassengerWithAppleDTO>> Handle(RegisterPassengerWithAppleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterPassengerWithAppleAsync(email: request.Email, firstName: request.FirstName, lastName: request.LastName, registerType: request.RegisterType, appleProviderKey: request.AppleProviderKey, phoneNumber: string.Empty,
                 referralCode: request.referralCode, points: request.Points);

            if (result.IsFailure)
            {
                return Result<RegisterPassengerWithAppleDTO>.Failure(error: result.Error);
            }


            var userDto = new RegisterPassengerWithAppleDTO
            {
                UserId = result.Value.UserId ?? Guid.Empty,
                Token = result.Value.Token ?? string.Empty,
                Role = request.RegisterType.ToString()
            };

            return Result<RegisterPassengerWithAppleDTO>.Success(userDto);
        }
    }
}
