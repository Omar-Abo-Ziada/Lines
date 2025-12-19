using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs;
using Lines.Application.Features.Users.GetUserMailDataById.DTOs;
using Lines.Application.Features.Users.LoginUser.DTOs;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminLine.Services;

/// <summary>
/// Admin-specific implementation of IApplicationUserService that doesn't require MediatR.
/// This is a simplified version for admin operations that don't need OTP/email workflows.
/// </summary>
public class AdminApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Driver> _driverRepository;
    private readonly IRepository<Passenger> _passengerRepository;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDBContext _context;

    public AdminApplicationUserService(
        UserManager<ApplicationUser> userManager,
        IRepository<Driver> driverRepository,
        IRepository<Passenger> passengerRepository,
        IConfiguration configuration,
        ApplicationDBContext context)
    {
        _userManager = userManager;
        _driverRepository = driverRepository;
        _passengerRepository = passengerRepository;
        _configuration = configuration;
        _context = context;
    }

    public async Task<(bool Succeeded, string[] Errors)> RegisterDriverAsync(Driver driver, string passwordHash, int registerRewardingPoints)
    {
        var appUser = new ApplicationUser
        {
            UserName = driver.Email,
            Email = driver.Email,
            PhoneNumber = driver.PhoneNumber,
            IsDeleted = false,
            IsActive = true,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PasswordHash = passwordHash,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        };

        var result = await _userManager.CreateAsync(appUser);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        appUser.DriverId = driver.Id;
        var updateResult = await _userManager.UpdateAsync(appUser);

        if (!updateResult.Succeeded)
        {
            return (false, updateResult.Errors.Select(e => e.Description).ToArray());
        }

        driver.AddPoints(registerRewardingPoints);
        await _userManager.AddToRoleAsync(appUser, "Driver");

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
            PasswordHash = _userManager.PasswordHasher.HashPassword(null, password),
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

        await _userManager.AddToRoleAsync(appUser, "Passenger");

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

            string role = user.PassengerId != null ? "Passenger" : user.DriverId != null ? "Driver" : "Admin";
            var token = await GenerateJwtToken(user);

            return new LoginDTO(token, role);
        }
        catch (Exception ex)
        {
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

        string role = user.PassengerId != null ? "Passenger" : user.DriverId != null ? "Driver" : "Admin";
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

        string role = user.PassengerId != null ? "Passenger" : user.DriverId != null ? "Driver" : "Admin";

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
        };

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
            return Result.Failure(Error.General);
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
            return Result.Failure(new Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
        }

        var result = await _userManager.RemovePasswordAsync(user);
        if (result.Succeeded)
        {
            result = await _userManager.AddPasswordAsync(user, newPassword);
        }

        if (!result.Succeeded)
        {
            return Result.Failure(new Error("400",
                string.Join(" , ", result.Errors.Select(e => e.Description)), ErrorType.Validation));
        }

        return Result.Success();
    }

    // Admin version: Simplified - doesn't send OTP emails
    public async Task<Result> UpdateDriverContactAsync(Guid userId, string? email, string? phoneNumber)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure(new Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
            }

            if (user.DriverId == null || user.DriverId == Guid.Empty)
            {
                return Result.Failure(new Error("400", "User is not a driver.", ErrorType.Validation));
            }

            if (!string.IsNullOrEmpty(email))
            {
                var existingUserWithEmail = await _userManager.FindByEmailAsync(email);
                if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
                {
                    return Result.Failure(new Error("400", "Email is already in use by another user.", ErrorType.Validation));
                }
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var existingUserWithPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Id != userId);
                if (existingUserWithPhone != null)
                {
                    return Result.Failure(new Error("400", "Phone number is already in use by another user.", ErrorType.Validation));
                }
            }

            var driver = await _driverRepository.Get(d => d.Id == user.DriverId.Value).FirstOrDefaultAsync();
            if (driver == null)
            {
                return Result.Failure(new Error("400", "Driver not found.", ErrorType.NotFound));
            }

            bool emailChanged = !string.IsNullOrEmpty(email) && user.Email != email;
            bool phoneChanged = !string.IsNullOrEmpty(phoneNumber) && user.PhoneNumber != phoneNumber;

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

            if (emailChanged)
            {
                user.EmailConfirmed = false; // Admin can manually confirm later
            }
            if (phoneChanged)
            {
                user.PhoneNumberConfirmed = false;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.Failure(new Error("400",
                    string.Join(" , ", updateResult.Errors.Select(e => e.Description)), ErrorType.Validation));
            }

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
                return Result.Failure(new Error("400", ex.Message, ErrorType.Validation));
            }

            // Note: Admin version doesn't send OTP emails automatically
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("500", $"An error occurred while updating contact information: {ex.Message}", ErrorType.Failure));
        }
    }

    // Admin version: Simplified - doesn't send OTP emails
    public async Task<Result> UpdateUserEmailAsync(Guid userId, string currentEmail, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(new Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
        }

        if (!string.IsNullOrEmpty(newEmail))
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(newEmail);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != userId)
            {
                return Result.Failure(new Error("400", "Email is already in use by another user.", ErrorType.Validation));
            }
        }

        Passenger? passenger = null;
        Driver? driver = null;

        if (user.PassengerId.HasValue && user.PassengerId != Guid.Empty)
        {
            passenger = await _passengerRepository.Get(d => d.Id == user.PassengerId.Value)
                .FirstOrDefaultAsync();
            if (passenger == null)
                return Result.Failure(new Error("400", "Passenger not found.", ErrorType.NotFound));
        }

        if (user.DriverId.HasValue && user.DriverId != Guid.Empty)
        {
            driver = await _driverRepository.Get(d => d.Id == user.DriverId.Value)
                .FirstOrDefaultAsync();
            if (driver == null)
                return Result.Failure(new Error("400", "Driver not found.", ErrorType.NotFound));
        }

        if (!string.IsNullOrEmpty(newEmail))
        {
            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpperInvariant();
            user.UserName = newEmail;
            user.NormalizedUserName = newEmail.ToUpperInvariant();
            user.EmailConfirmed = false; // Admin can manually confirm later
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Result.Failure(new Error("400",
                string.Join(" , ", updateResult.Errors.Select(e => e.Description)), ErrorType.Validation));
        }

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
            return Result.Failure(new Error("400", ex.Message, ErrorType.Validation));
        }

        // Note: Admin version doesn't send OTP emails automatically
        return Result.Success();
    }

    public async Task<Guid?> IsDriverAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        if (user.DriverId is null || user.DriverId == Guid.Empty)
            return Guid.Empty;

        return user.DriverId;
    }

    public async Task<Guid?> IsPaasengerAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        if (user.PassengerId is null || user.PassengerId == Guid.Empty)
            return Guid.Empty;

        return user.PassengerId;
    }

    public VechileInfoDTO? GetVechileKmPriceForUser(Guid userId)
    {
        var query =
            from user in _context.ApplicationUsers
            join driver in _context.Drivers
                on user.DriverId equals driver.Id into driverGroup
            from driver in driverGroup.DefaultIfEmpty()
            join vehicle in _context.Vehicles
                on driver.Id equals vehicle.DriverId into vehicleGroup
            from vehicle in vehicleGroup.DefaultIfEmpty()
            where user.Id == userId && (vehicle == null || vehicle.IsPrimary)
            select new VechileInfoDTO
            {
                KmPrice = vehicle != null ? vehicle.KmPrice : (decimal?)null,
                DriverId = driver != null ? driver.Id : Guid.Empty
            };

        return query.FirstOrDefault();
    }

    public async Task<Result<LoginDTO>> RegisterUserWithGoogleAsync(string email, string firstName, string lastName, string phoneNumber, string googleProviderKey, RegisterType registerType, string referralCode, int points)
    {
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

        var createUserResult = await _userManager.CreateAsync(appUser);

        if (!createUserResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE.CREATE_USER", createUserResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user", ErrorType.Failure));
        }

        var loginInfo = new UserLoginInfo("Google", googleProviderKey, "Google");
        var addLoginResult = await _userManager.AddLoginAsync(appUser, loginInfo);

        if (!addLoginResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE_LOGIN.AlREADY_LINKED", addLoginResult.Errors.FirstOrDefault()?.Description ?? "Failed to link Google login", ErrorType.Failure));
        }

        if (registerType == RegisterType.Driver)
        {
            var driver = new Driver(appUser.Id, firstName, lastName, new Email(email), new PhoneNumber(phoneNumber), isNotifiedForOnlyTripsAboveMyPrice: false);
            await _driverRepository.AddAsync(driver);
            appUser.DriverId = driver.Id;
            await _userManager.UpdateAsync(appUser);
            await _userManager.AddToRoleAsync(appUser, "Driver");
        }
        else
        {
            var passenger = new Passenger(appUser.Id, firstName, lastName, email: new Email(email), phoneNumber: new PhoneNumber(phoneNumber), referralCode: referralCode);
            passenger.AddPoints(points);
            await _passengerRepository.AddAsync(passenger);
            appUser.PassengerId = passenger.Id;
            await _userManager.UpdateAsync(appUser);
            await _userManager.AddToRoleAsync(appUser, "Passenger");
        }

        LoginDTO loginDto = await LoginGoogleAsync(email);

        if (loginDto is null)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.GOOGLE.LOGIN", "Failed to log in", ErrorType.Failure));
        }

        return Result<LoginDTO>.Success(loginDto);
    }

    public async Task<Result<LoginDTO>> RegisterPassengerWithAppleAsync(string email, string firstName, string lastName, string phoneNumber, string appleProviderKey, RegisterType registerType, string referralCode, int points)
    {
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

        var createUserResult = await _userManager.CreateAsync(appUser);

        if (!createUserResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.CREATE_USER", createUserResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user", ErrorType.Failure));
        }

        var loginInfo = new UserLoginInfo("Apple", appleProviderKey, "Apple");
        var addLoginResult = await _userManager.AddLoginAsync(appUser, loginInfo);

        if (!addLoginResult.Succeeded)
        {
            return Result<LoginDTO>.Failure(error: new Error("REGISTER.APPLE.AlREADY_LINKED", addLoginResult.Errors.FirstOrDefault()?.Description ?? "Failed to link Apple login", ErrorType.Failure));
        }

        var passenger = new Passenger(appUser.Id, firstName, lastName, email: new Email(email), phoneNumber: new PhoneNumber(phoneNumber), referralCode: referralCode);
        passenger.AddPoints(points);
        await _passengerRepository.AddAsync(passenger);
        appUser.PassengerId = passenger.Id;
        await _userManager.UpdateAsync(appUser);
        await _userManager.AddToRoleAsync(appUser, "Passenger");

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

    public async Task<Result> UpdatePassenerProfileAsync(Guid userId, string? firstName, string? lastName, string? phoneNumber)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure(new Error("400", $"User with ID '{userId}' not found.", ErrorType.NotFound));
            }

            if (user.PassengerId == null || user.PassengerId == Guid.Empty)
            {
                return Result.Failure(new Error("400", "User is not a Passenger.", ErrorType.Validation));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var existingUserWithPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Id != userId);
                if (existingUserWithPhone != null)
                {
                    return Result.Failure(new Error("400", "Phone number is already in use by another user.", ErrorType.Validation));
                }
            }

            var passenger = await _passengerRepository.Get(d => d.Id == user.PassengerId.Value).FirstOrDefaultAsync();
            if (passenger == null)
            {
                return Result.Failure(new Error("400", "Passenger not found.", ErrorType.NotFound));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var newPhoneNumber = new PhoneNumber(phoneNumber);
                user.PhoneNumber = newPhoneNumber.ToString();
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.Failure(new Error("400",
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
                return Result.Failure(new Error("400", ex.Message, ErrorType.Validation));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("500", $"An error occurred while updating contact information: {ex.Message}", ErrorType.Failure));
        }
    }

    public async Task<Guid?> GetUserIdByMailAsync(string email)
    {
        var userId = await _userManager
            .SelectWhere(_context, u => u.Email == email, u => u.Id);

        var first = userId.FirstOrDefault();

        return first == Guid.Empty ? null : first;
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

