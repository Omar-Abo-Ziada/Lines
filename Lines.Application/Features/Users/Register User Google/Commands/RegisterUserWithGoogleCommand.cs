using Lines.Application.Features.Users.Register_User.DTOs;

namespace Lines.Application.Features.Users.Register_User_Google.Commands;
public record RegisterUserWithGoogleCommand(string FirstName, string LastName, string Email, RegisterType RegisterType, string googleProviderKey,
    string referralCode , int Points) : IRequest<Result<RegisterUserDTO>>;

public class RegisterUserCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<RegisterUserWithGoogleCommand, Result<RegisterUserDTO>>(parameters)
{
    private readonly IApplicationUserService _authService = authService;
    public override async Task<Result<RegisterUserDTO>> Handle(RegisterUserWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterUserWithGoogleAsync(email: request.Email, firstName: request.FirstName, lastName: request.LastName, registerType: request.RegisterType, googleProviderKey: request.googleProviderKey, phoneNumber: string.Empty,
             referralCode: request.referralCode, points: request.Points);

        if (result.IsFailure)
        {
            return Result<RegisterUserDTO>.Failure(error: result.Error);
        }

        var userDto = new RegisterUserDTO
        {
            UserId = result.Value.UserId ?? Guid.Empty,
            Token = result.Value.Token ?? string.Empty,
            Role = request.RegisterType.ToString()
        };

        return Result<RegisterUserDTO>.Success(userDto);
    }
}
