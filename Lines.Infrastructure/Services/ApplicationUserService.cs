using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs;
using Lines.Application.Features.Users.GetUserMailDataById.DTOs;
using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Application.Features.Otps.CreateOtp.Orchestrators;
using Lines.Application.Common.Email;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Passengers;
using Lines.Domain.Shared;
using Lines.Domain.Value_Objects;
using Lines.Infrastructure.Context;
using Lines.Infrastructure.Extensions;
using Lines.Infrastructure.Identity;
using Lines.Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using MediatR;
using Lines.Application.Features.Users.GetUserById.Orchestrators;
using Lines.Domain.Constants;
namespace Lines.Infrastructure.Services;

public class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Driver> _driverRepository;
    private readonly IRepository<Passenger> _passengerRepository;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDBContext _context;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    public ApplicationUserService(UserManager<ApplicationUser> userManager, IRepository<Driver> driverRepository, IRepository<Passenger> passengerRepository, IConfiguration configuration,
        ApplicationDBContext context, IEmailService emailService, IMediator mediator)
    {
        _userManager = userManager;
        _driverRepository = driverRepository;
        _passengerRepository = passengerRepository;
        _configuration = configuration;
        _context = context;
        _emailService = emailService;
        _mediator = mediator;
    }

    public async Task<(bool Succeeded, string[] Errors)> RegisterDriverAsync(Driver driver, string passwordHash, int registerRewardingPoints)
    {
        var appUser = new ApplicationUser
        {
            UserName = driver.Email,
            Email = driver.Email,
            PhoneNumber = driver.PhoneNumber,
            // Don't set DriverId yet - will update after creation to avoid FK constraint
            IsDeleted = false,
            IsActive = true,
            EmailConfirmed = true, // Email is already verified in the registration process
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PasswordHash = passwordHash, // Use pre-hashed password
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        };

        var result = await _userManager.CreateAsync(appUser); // No password parameter since it's pre-hashed

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        // Now update the ApplicationUser with DriverId after successful creation
        appUser.DriverId = driver.Id;
        var updateResult = await _userManager.UpdateAsync(appUser);

        if (!updateResult.Succeeded)
        {
            return (false, updateResult.Errors.Select(e => e.Description).ToArray());
        }

        // Add points to the existing driver entity
        driver.AddPoints(registerRewardingPoints);

        // Add to 'Driver' role
        await _userManager.AddToRoleAsync(appUser, Roles.Driver);

        return (true, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string[] Errors, Guid? userId)> RegisterPassengerAsync(Passenger passenger, string password)
    {

        var entity = await _passengerRepository.AddAsync(passenger);

        var appUser = new ApplicationUser
        {
            UserName = passenger.Email,
            NormalizedUserName = passenger.Email.ToString().ToUpperInvariant(),
            Email = passenger.Email,
            PhoneNumber = passenger.PhoneNumber,
            IsDeleted = false,
            IsActive = true,
            PassengerId = entity.Id,
            NormalizedEmail = entity.Email.ToString().ToUpperInvariant(),
            EmailConfirmed = false,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PasswordHash = _userManager.PasswordHasher.HashPassword(null, password), // Hash the password
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        };
        
        var result = await _userManager.CreateAsync(appUser, password);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray(), null);
        }

        // Add to 'Passenger' role
        await _userManager.AddToRoleAsync(appUser, Roles.Passenger);

        return (true, Array.Empty<string>(), appUser.Id);
    }

    public async Task<LoginDTO> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password) || !user.EmailConfirmed)
            {
                return new LoginDTO(string.Empty, string.Empty);
            }

            string role = user.PassengerId != null ? Roles.Passenger : user.DriverId != null ? Roles.Driver : Roles.Admin;
            var token = await GenerateJwtToken(user);

            return new LoginDTO(token, role);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message;
            return new LoginDTO(string.Empty, string.Empty);
        }

    }

    public async Task<LoginDTO> LoginGoogleAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !user.EmailConfirmed)
        {
            return new LoginDTO(string.Empty, string.Empty, Guid.Empty);
        }

        string role = user.PassengerId != null ? Roles.Passenger : user.DriverId != null ? Roles.Driver : Roles.Admin;
        var token = await GenerateJwtToken(user);

        return new LoginDTO(token, role, user.Id);
    }
    public async Task<LoginDTO> LoginAppleAsync(string email)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !user.EmailConfirmed)
        {
            return new LoginDTO(string.Empty, string.Empty, Guid.Empty);
        }

        string role = user.PassengerId != null ? Roles.Passenger : user.DriverId != null ? Roles.Driver : Roles.Admin;

        var token = await GenerateJwtToken(user);

        return new LoginDTO(token, role, user.Id);
    }


    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("SecurityStamp", user.SecurityStamp?? string.Empty)
            };

        // Add PassengerId and DriverId if they exist
        if (user.PassengerId != null)
            claims.Add(new Claim("passengerId", user.PassengerId.ToString()));

        if (user.DriverId != null)
            claims.Add(new Claim("driverId", user.DriverId.ToString()));

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(bool Succeeded, string[] Errors)> DeleteByPassengerIdAsync(Guid passengerId)
    {
        var user = await _userManager.FindByConditionAsync(_context, user => user.PassengerId == passengerId);
        if (user is null)
        {
            return (false, new[] { $"User with passenger id '{passengerId}' not found." });
        }

        user.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        return (true, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string[] Errors)> DeleteAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return (false, new[] { $"User with ID '{userId}' not found." });
        }

        user.IsDeleted = true;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        return (true, Array.Empty<string>());
    }

    public async Task<Guid> GetUserIdByPassengerId(Guid passengerId)
    {
        var user = await _userManager.FindByConditionAsync(_context, user => user.PassengerId == passengerId);

        if (user is null)
        {
            throw new Exception($"User with passenger id '{passengerId}' not found.");
        }

        return user.Id;
    }

    public async Task<Result> ConfirmMailAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            throw new Exception($"User with user id '{userId}' not found.");
        }

        user.EmailConfirmed = true;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result.Failure(Application.Shared.Error.General);
        }

        return Result.Success();
    }

    public async Task<UserMailDataDto?> GetMailDataByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return new UserMailDataDto
        {
            Mail = user?.Email ?? string.Empty,
            UserName = user?.UserName ?? string.Empty
        };

    }

    public async Task<Result> UpdatePasswordAsync(Guid userId, string newPassword, string? currentPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(new Application.Shared.Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
        }

        // Verify current password
        //if (currentPassword is not null && !await _userManager.CheckPasswordAsync(user, currentPassword)) // in case user change password not forget
        //{
        //    return Result.Failure(
        //               new Error("400", "Current password is incorrect.", ErrorType.Validation));
        //}

        var result = await _userManager.RemovePasswordAsync(user);
        if (result.Succeeded)
        {
            result = await _userManager.AddPasswordAsync(user, newPassword);
        }

        if (!result.Succeeded)
        {
            return Result.Failure(new Application.Shared.Error("400",
                          string.Join(" , ", result.Errors.Select(e => e.Description)), ErrorType.Validation));
        }

        return Result.Success();
    }

    public async Task<Guid?> GetUserIdByMailAsync(string email)
    {
        var userId = await _userManager
                            .SelectWhere(_context, u => u.Email == email, u => u.Id);

        var first = userId.FirstOrDefault();

        return first == Guid.Empty ? null : first;

    }

    public async Task<Result> UpdateDriverContactAsync(Guid userId, string? email, string? phoneNumber)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure(new Application.Shared.Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
            }

            // Check if user is a driver
            if (user.DriverId == null || user.DriverId == Guid.Empty)
            {
                return Result.Failure(new Application.Shared.Error("400", "User is not a driver.", ErrorType.Validation));
            }

            // Check email uniqueness (excluding current user) - only if email is provided
            if (!string.IsNullOrEmpty(email))
            {
                var existingUserWithEmail = await _userManager.FindByEmailAsync(email);
                if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
                {
                    return Result.Failure(new Application.Shared.Error("400", "Email is already in use by another user.", ErrorType.Validation));
                }
            }

            // Check phone uniqueness (excluding current user) - only if phone is provided
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var existingUserWithPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Id != userId);
                if (existingUserWithPhone != null)
                {
                    return Result.Failure(new Application.Shared.Error("400", "Phone number is already in use by another user.", ErrorType.Validation));
                }
            }

            // Get driver entity
            var driver = await _driverRepository.Get(d => d.Id == user.DriverId.Value).FirstOrDefaultAsync();
            if (driver == null)
            {
                return Result.Failure(new Application.Shared.Error("400", "Driver not found.", ErrorType.NotFound));
            }

            // Check if email changed - only if email is provided
            bool emailChanged = !string.IsNullOrEmpty(email) && user.Email != email;
            bool phoneChanged = !string.IsNullOrEmpty(phoneNumber) && user.PhoneNumber != phoneNumber;

            // Update ApplicationUser - only update provided fields
            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
                user.NormalizedEmail = email.ToUpperInvariant();
                user.UserName = email;
                user.NormalizedUserName = email.ToUpperInvariant();
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                user.PhoneNumber = phoneNumber;
            }

            // Reset verification flags if contact info changed
            if (emailChanged)
            {
                user.EmailConfirmed = false;
            }
            if (phoneChanged)
            {
                user.PhoneNumberConfirmed = false;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.Failure(new Application.Shared.Error("400",
                    string.Join(" , ", updateResult.Errors.Select(e => e.Description)), ErrorType.Validation));
            }

            // Update Driver entity - only update provided fields
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var newEmail = new Email(email);
                    driver.Email = newEmail;
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var newPhoneNumber = new PhoneNumber(phoneNumber);
                    driver.PhoneNumber = newPhoneNumber;
                }

                await _driverRepository.UpdateAsync(driver);
                _driverRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from value objects
                return Result.Failure(new Application.Shared.Error("400", ex.Message, ErrorType.Validation));
            }

            // Send OTP email if email was changed
            if (emailChanged)
            {
                try
                {
                    // Create OTP for the user
                    var otpResult = await _mediator.Send(new CreateOtpOrchestrator(userId));
                    if (otpResult.IsFailure)
                    {
                        // Log error but don't fail the contact update
                        // The email was already updated successfully
                        return Result.Success();
                    }

                    // Get user mail data
                    var mailDataResult = await _mediator.Send(new GetUserMailDataByIdOrchestrator(userId));
                    if (mailDataResult.IsFailure)
                    {
                        return Result.Success(); // Don't fail contact update
                    }

                    // Send OTP email
                    MailData mailData = new MailData(
                        mailDataResult.Value.Mail,
                        mailDataResult.Value.UserName,
                        MailSubjects.Otp,
                        string.Format(MailFormat.Otp.Html, mailDataResult.Value.UserName, otpResult.Value.Code)
                    );

                    var sendMailResult = await _emailService.SendMailAsync(mailData);
                    if (!sendMailResult.Succeeded)
                    {
                        // Log error but don't fail the contact update
                        // The email was already updated successfully
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the contact update
                    // The email was already updated successfully
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Application.Shared.Error("500", $"An error occurred while updating contact information: {ex.Message}", ErrorType.Failure));
        }
    }
    public async Task<Result> UpdateUserEmailAsync(Guid userId, string currentEmail, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(new Application.Shared.Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
        }



        // Check email uniqueness (excluding current user) - only if email is provided
        if (!string.IsNullOrEmpty(newEmail))
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(newEmail);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
            {
                return Result.Failure(new Application.Shared.Error("400", "Email is already in use by another user.", ErrorType.Validation));
            }
        }


        //  Prepare domain entities (passenger/driver) if exist — fetch once each (if present)
        Passenger? passenger = null;
        Driver? driver = null;

        if (user.PassengerId.HasValue && user.PassengerId != Guid.Empty)
        {
            passenger = await _passengerRepository.Get(d => d.Id == user.PassengerId.Value)
                                                 .FirstOrDefaultAsync();
            if (passenger == null)
                return Result.Failure(new Application.Shared.Error("400", "Passenger not found.", ErrorType.NotFound));
        }

        if (user.DriverId.HasValue && user.DriverId != Guid.Empty)
        {
            driver = await _driverRepository.Get(d => d.Id == user.DriverId.Value)
                                           .FirstOrDefaultAsync();
            if (driver == null)
                return Result.Failure(new Application.Shared.Error("400", "Driver not found.", ErrorType.NotFound));
        }




        ///





        // Update ApplicationUser - only update provided fields
        if (!string.IsNullOrEmpty(newEmail))
        {

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpperInvariant();
            user.UserName = newEmail;
            user.NormalizedUserName = newEmail.ToUpperInvariant();
            user.EmailConfirmed = false;
        }



        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Failure(new Application.Shared.Error("400",
                string.Join(" , ", updateResult.Errors.Select(e => e.Description)), ErrorType.Validation));
        }

        // Update domain entities (Passenger or  Driver)  
        try
        {
            if (!string.IsNullOrEmpty(newEmail))
            {
                var email = new Email(newEmail);



                if (passenger is not null)
                {
                    passenger.Email = email;
                    await _passengerRepository.UpdateAsync(passenger);
                    await _passengerRepository.SaveChangesAsync();
                }

                if (driver is not null)
                {
                    driver.Email = email;
                    await _driverRepository.UpdateAsync(driver);
                    await _driverRepository.SaveChangesAsync();
                }
            }




        }
        catch (ArgumentException ex)
        {
            // Handle validation errors from value objects
            return Result.Failure(new Application.Shared.Error("400", ex.Message, ErrorType.Validation));
        }

        // Send OTP email if email was changed

        try
        {
            // Create OTP for the user
            var otpResult = await _mediator.Send(new CreateOtpOrchestrator(userId));
            if (otpResult.IsFailure)
            {
                // Log error but don't fail the contact update
                // The email was already updated successfully
                return Result.Success();
            }

            // Get user mail data
            var mailDataResult = await _mediator.Send(new GetUserMailDataByIdOrchestrator(userId));
            if (mailDataResult.IsFailure)
            {
                return Result.Success(); // Don't fail contact update
            }

            // Send OTP email
            MailData mailData = new MailData(
                mailDataResult.Value.Mail,
                mailDataResult.Value.UserName,
                MailSubjects.Otp,
                string.Format(MailFormat.Otp.Html, mailDataResult.Value.UserName, otpResult.Value.Code)
            );

            var sendMailResult = await _emailService.SendMailAsync(mailData);
            if (!sendMailResult.Succeeded)
            {
                // Log error but don't fail the contact update
                // The email was already updated successfully
            }
        }
        catch (Exception ex)
        {
            // Log error but don't fail the contact update
            // The email was already updated successfully
        }


        return Result.Success();



    }

    public async Task<Guid?> IsDriverAsync(Guid userId)
    {
        var user = await _userManager
                            .FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        if (user.DriverId is null || user.DriverId == Guid.Empty)
            return Guid.Empty;

        return user.DriverId;
    }

    public async Task<Result> UpdateStripeCustomerIdAsync(Guid userId, string stripeCustomerId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(new Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
        }

        // Check email uniqueness (excluding current user) - only if email is provided
        if (string.IsNullOrEmpty(stripeCustomerId))
        {
            return Result.Failure(Error.NullValue);
        }

        user.setStripeCustomerId(stripeCustomerId);
        return Result.Success();
    }
 
    public async Task<Guid?> IsPaasengerAsync(Guid userId)
    {
        var user = await _userManager
                            .FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        if (user.PassengerId is null || user.PassengerId == Guid.Empty)
            return Guid.Empty;

        return user.PassengerId;
    }

    /// <summary>
    /// If the user is driver and has vechile => will return the km price and driver id for this vechile
    /// else return null
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public VechileInfoDTO? GetVechileKmPriceForUser(Guid userId)
    {
        var query =
            from user in _context.ApplicationUsers
            join driver in _context.Drivers
                on user.DriverId equals driver.Id into driverGroup
            from driver in driverGroup.DefaultIfEmpty() // LEFT JOIN driver
            join vehicle in _context.Vehicles
                on driver.Id equals vehicle.DriverId into vehicleGroup
            from vehicle in vehicleGroup.DefaultIfEmpty() // LEFT JOIN vehicle (primary vehicle)
            where user.Id == userId && (vehicle == null || vehicle.IsPrimary)
            select new VechileInfoDTO
            {
                KmPrice = vehicle != null ? vehicle.KmPrice : (decimal?)null,
                DriverId = driver != null ? driver.Id : Guid.Empty
            };

        var result = query.FirstOrDefault();

        return result;


        //return _context.Set<ApplicationUser>()   // Ensures it's DbSet
        //    .Where(u => u.Id == userId)
        //    .Select(u => new VechileInfoDTO
        //    {
        //        KmPrice = u.Driver != null && u.Driver.Vehicle != null
        //                    ? (decimal?)u.Driver.Vehicle.KmPrice
        //                    : null,
        //        DriverId = u.DriverId ?? Guid.Empty
        //    })
        //    .FirstOrDefault();
    }

    public async Task<Result<LoginDTO>> RegisterUserWithGoogleAsync(string email, string firstName, string lastName, string phoneNumber, string googleProviderKey, RegisterType registerType,
        string referralCode, int points)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE.USER_ALREADY_EXISTS", "User already exists.", ErrorType.Validation));
        }

        var appUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            PhoneNumber = phoneNumber,
            EmailConfirmed = true,
            IsActive = true,
            IsDeleted = false,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        var createUserResult = await _userManager.CreateAsync(appUser); // no password

        if (!createUserResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE.CREATE_USER", createUserResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user", ErrorType.Failure));
        }

        // Link external login (Google)
        // Create an object that represents the external login (Google in this case)
        var loginInfo = new UserLoginInfo("Google", googleProviderKey, "Google");

        // Link this Google login to the existing ApplicationUser in the AspNetUserLogins table
        // This way, ASP.NET Identity knows this user can log in with Google or with any other future provider
        var addLoginResult = await _userManager.AddLoginAsync(appUser, loginInfo);

        // If linking the external login failed (e.g., already linked to another user), return the error
        if (!addLoginResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE_LOGIN.AlREADY_LINKED", addLoginResult.Errors.FirstOrDefault()?.Description ?? "Failed to link Google login", ErrorType.Failure));
        }


        // Create your domain entity (Driver or Passenger)
        if (registerType == RegisterType.Driver)
        {
            var driver = new Driver(appUser.Id, firstName, lastName, new Email(email), new PhoneNumber(phoneNumber), isNotifiedForOnlyTripsAboveMyPrice: false);

            await _driverRepository.AddAsync(driver);
            appUser.DriverId = driver.Id;
            await _userManager.UpdateAsync(appUser);

            // Assign role
            await _userManager.AddToRoleAsync(appUser, Roles.Driver);

        }
        else
        {
            var passenger = new Passenger(appUser.Id, firstName, lastName, email: new Email(email), phoneNumber: new PhoneNumber(phoneNumber),
                referralCode: referralCode);

            passenger.AddPoints(points);
            await _passengerRepository.AddAsync(passenger);

            appUser.PassengerId = passenger.Id;
            await _userManager.UpdateAsync(appUser);

            // Assign role
            await _userManager.AddToRoleAsync(appUser, Roles.Passenger);
        }

        LoginDTO loginDto = await LoginGoogleAsync(email);

        if (loginDto is null)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE.LOGIN", "Failed to log in", ErrorType.Failure));
        }

        return Result<LoginDTO>.Success(loginDto);
    }

    public async Task<Result<LoginDTO>> RegisterPassengerWithAppleAsync(string email, string firstName, string lastName, string phoneNumber, string appleProviderKey, RegisterType registerType,
         string referralCode, int points)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.USER_ALREADY_EXISTS", "User already exists.", ErrorType.Validation));
        }

        var appUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            PhoneNumber = phoneNumber,
            EmailConfirmed = true,
            IsActive = true,
            IsDeleted = false,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        var createUserResult = await _userManager.CreateAsync(appUser); // no password

        if (!createUserResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.CREATE_USER", createUserResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user", ErrorType.Failure));
        }

        // Link external login (Apple)
        // Create an object that represents the external login (Apple in this case)
        var loginInfo = new UserLoginInfo("Apple", appleProviderKey, "Apple");

        // Link this Apple login to the existing ApplicationUser in the AspNetUserLogins table
        // This way, ASP.NET Identity knows this user can log in with Apple or with any other future provider
        var addLoginResult = await _userManager.AddLoginAsync(appUser, loginInfo);

        // If linking the external login failed (e.g., already linked to another user), return the error
        if (!addLoginResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.AlREADY_LINKED", addLoginResult.Errors.FirstOrDefault()?.Description ?? "Failed to link Apple login", ErrorType.Failure));
        }

        ////////////
        ///
        var passenger = new Passenger(appUser.Id, firstName, lastName, email: new Email(email), phoneNumber: new PhoneNumber(phoneNumber),
        referralCode: referralCode);

        passenger.AddPoints(points);
        await _passengerRepository.AddAsync(passenger);

        appUser.PassengerId = passenger.Id;
        await _userManager.UpdateAsync(appUser);

        // Assign role
        await _userManager.AddToRoleAsync(appUser, Roles.Passenger);

        //////////////

        LoginDTO loginDto = await LoginAppleAsync(email);

        if (loginDto is null)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.LOGIN", "Failed to log in", ErrorType.Failure));
        }

        return Result<LoginDTO>.Success(loginDto);

    }
    public async Task<GetPassengerAndDriverIdsDTO?> GetPassengerAndDriverIdsByUserIdAsync(Guid userId)
    {
        var user = await _userManager.SelectWhere(_context, u => u.Id == userId,
                                                    u => new { u.PassengerId, u.DriverId, u.IsDeleted });

        var first = user.FirstOrDefault();
        GetPassengerAndDriverIdsDTO getPassengerAndDriverIdsDTO = new(userId, first?.PassengerId, first?.DriverId, first.IsDeleted);

        if (getPassengerAndDriverIdsDTO == null || getPassengerAndDriverIdsDTO.IsDeleted)
        {
            return null;
        }

        return getPassengerAndDriverIdsDTO;
    }

    public string HashPassword(string password)
    {
        return _userManager.PasswordHasher.HashPassword(null, password);
    }

    public async Task<Result> UpdatePassenerProfileAsync(
Guid userId,
string? firstName,
string? lastName, string? phoneNumber)
    {



        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure(new Application.Shared.Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
            }

            if (user.PassengerId == null || user.PassengerId == Guid.Empty)
            {
                return Result.Failure(new Application.Shared.Error("400", "User is not a Passenger.", ErrorType.Validation));
            }



            // Check phone uniqueness (excluding current user) - only if phone is provided
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var existingUserWithPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Id != userId);
                if (existingUserWithPhone != null)
                {
                    return Result.Failure(new Application.Shared.Error("400", "Phone number is already in use by another user.", ErrorType.Validation));
                }
            }

            // Get passenger entity
            var passenger = await _passengerRepository.Get(d => d.Id == user.PassengerId.Value).FirstOrDefaultAsync();
            if (passenger == null)
            {
                return Result.Failure(new Application.Shared.Error("400", "Passenger not found.", ErrorType.NotFound));
            }



            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var newPhoneNumber = new PhoneNumber(phoneNumber);
                user.PhoneNumber = newPhoneNumber.ToString();
            }



            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.Failure(new Application.Shared.Error("400",
                    string.Join(" , ", updateResult.Errors.Select(e => e.Description)), ErrorType.Validation));
            }


            try
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    passenger.FirstName = firstName;

                if (!string.IsNullOrWhiteSpace(lastName))
                    passenger.LastName = lastName;




                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    var newPhoneNumber = new PhoneNumber(phoneNumber);
                    passenger.PhoneNumber = newPhoneNumber;
                }

                await _passengerRepository.UpdateAsync(passenger);
                _passengerRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                // Handle validation errors from value objects
                return Result.Failure(new Application.Shared.Error("400", ex.Message, ErrorType.Validation));
            }

            //catch (Exception ex)
            //{
            //    // Log error but don't fail the contact update
            //    // The email was already updated successfully
            //}


            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Application.Shared.Error("500", $"An error occurred while updating contact information: {ex.Message}", ErrorType.Failure));
        }

    }

    public async Task<Guid?> GetUserIdByDriverIdAsync(Guid driverId)
    {
        var userIds = await _userManager
            .SelectWhere(_context,
                u => u.DriverId == driverId,
                u => u.Id);

        var first = userIds.FirstOrDefault();

        return first == Guid.Empty ? (Guid?)null : first;
    }
    public async Task<Dictionary<Guid, Guid>> GetUserIdsByDriverIdsAsync(IEnumerable<Guid> driverIds)
    {
        var rows = await _userManager.SelectWhere(_context,
            u => u.DriverId != null && driverIds.Contains(u.DriverId.Value),
            u => new { DriverId = u.DriverId!.Value, UserId = u.Id });

        return rows.ToDictionary(x => x.DriverId, x => x.UserId);
    }



    public async Task<Guid?> GetUserIdByPassengerIdAsync(Guid passengerId)
    {
        var userIds = await _userManager
            .SelectWhere(_context,
                u => u.PassengerId == passengerId,
                u => u.Id);

        var first = userIds.FirstOrDefault();

        return first == Guid.Empty ? (Guid?)null : first;
    }
    public async Task<string?> GetStripeCustomerIdByUserIdAsync(Guid userId)
    {
        var stripeCustomerId = await _userManager
            .SelectWhere(_context,
                u => u.Id == userId,
                u => u.StripeCustomerId);

        var first = stripeCustomerId.FirstOrDefault();

        return first;
    }


}


