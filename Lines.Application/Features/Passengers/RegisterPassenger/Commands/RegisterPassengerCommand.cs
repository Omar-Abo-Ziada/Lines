using Lines.Domain.Models.Passengers;

namespace Lines.Application.Features.Passengers;

public record RegisterPassengerCommand(string FirstName, string LastName, string Email, string PhoneNumber, string Password,
                                       int registerRewardingPoints, string referralCode, bool isReferralCodeSubmitted)
                                        : IRequest<(bool Succeeded, string[] Errors, Guid? userId)>;

public class RegisterPassengerCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService)
    : RequestHandlerBase<RegisterPassengerCommand, (bool Succeeded, string[] Errors, Guid? userId)>(parameters)
{
    private readonly IApplicationUserService _authService = authService;
    public override async Task<(bool Succeeded, string[] Errors, Guid? userId)> Handle(RegisterPassengerCommand request, CancellationToken cancellationToken)
    {
        Email email = new(request.Email);
        PhoneNumber phoneNumber = new(request.PhoneNumber);

        var passenger = new Passenger(Guid.NewGuid(), request.FirstName, request.LastName, email, phoneNumber,  request.referralCode);
        passenger.isReferralCodeSubmitted = request.isReferralCodeSubmitted;  
        passenger.AddPoints(request.registerRewardingPoints);
        var result = await _authService.RegisterPassengerAsync(passenger, request.Password);
        return result;
    }
}