using Lines.Domain.Models.Drivers;
using Lines.Domain.Value_Objects;
using Lines.Application.Interfaces;

namespace Lines.Application.Features.Drivers;

public record RegisterDriverCommand(string FirstName, string LastName, string Email, string PhoneNumber, string Password
                                    , bool isNotifiedForOnlyTripsAboveMyPrice, int registerRewardingPoints) : IRequest<(bool Succeeded, string[] Errors)>;

public class RegisterDriverCommandHandler(RequestHandlerBaseParameters parameters, IApplicationUserService authService, IRepository<Driver> driverRepository)
    : RequestHandlerBase<RegisterDriverCommand, (bool Succeeded, string[] Errors)>(parameters)
{
    private readonly IApplicationUserService _authService = authService;
    private readonly IRepository<Driver> _driverRepository = driverRepository;
    
    public override async Task<(bool Succeeded, string[] Errors)> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
    {
        Email email = new(request.Email);
        PhoneNumber phoneNumber = new(request.PhoneNumber);

        var driver = new Driver(Guid.NewGuid(), request.FirstName, request.LastName, email, phoneNumber, request.isNotifiedForOnlyTripsAboveMyPrice);

        // Save Driver to database first
        await _driverRepository.AddAsync(driver, cancellationToken);
        _driverRepository.SaveChanges(); // Save early to ensure Driver exists before ApplicationUser creation

        // Hash the password before passing to RegisterDriverAsync
        var passwordHash = _authService.HashPassword(request.Password);
        var result = await _authService.RegisterDriverAsync(driver, passwordHash, request.registerRewardingPoints);
        return result;
    }
}