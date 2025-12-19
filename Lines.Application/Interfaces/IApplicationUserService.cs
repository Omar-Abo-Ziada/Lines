using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.Users.GetPassengerAndDriverIdsByUserId.DTOs;
using Lines.Application.Features.Users.GetUserMailDataById.DTOs;
using Lines.Application.Features.Users.LoginUser.DTOs;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.Passengers;

namespace Lines.Application.Interfaces;

public interface IApplicationUserService
{
    Task<Result> ConfirmMailAsync(Guid userId);
    Task<(bool Succeeded, string[] Errors)> DeleteAsync(Guid userId);
    Task<(bool Succeeded, string[] Errors)> DeleteByPassengerIdAsync(Guid passengerId);
    Task<UserMailDataDto?> GetMailDataByIdAsync(Guid userId);
    Task<Guid?> GetUserIdByMailAsync(string email);
    Task<Guid> GetUserIdByPassengerId(Guid passengerId);
    Task<Guid?> IsDriverAsync(Guid userId);
    Task<LoginDTO> LoginAsync(string email, string password);  // here >> error that logindto is not visible on domain level
    Task<LoginDTO> LoginGoogleAsync(string email);
    Task<LoginDTO> LoginAppleAsync(string email);
    Task<(bool Succeeded, string[] Errors)> RegisterDriverAsync(Driver driver, string passwordHash, int registerRewardingPoints);
    Task<(bool Succeeded, string[] Errors, Guid? userId)> RegisterPassengerAsync(Passenger passenger, string password);
    Task<Result<LoginDTO>> RegisterUserWithGoogleAsync(string email, string firstName, string lastName, string phoneNumber, string googleProviderKey, RegisterType registerType, string referralCode, int points);
    Task<Result<LoginDTO>> RegisterPassengerWithAppleAsync(string email, string firstName, string lastName, string phoneNumber, string appleProviderKey, RegisterType registerType,string referralCode, int points);
    Task<Result> UpdatePasswordAsync(Guid userId, string newPassword, string? currentPassword);
    Task<Result> UpdateDriverContactAsync(Guid userId, string? email, string? phoneNumber);
    Task<Result> UpdateUserEmailAsync(Guid userId, string currentEmail, string newEmail);
    Task<Result> UpdatePassenerProfileAsync(Guid userId, string? firstName, string? lastName, string? phoneNumber);
    Task<GetPassengerAndDriverIdsDTO?> GetPassengerAndDriverIdsByUserIdAsync(Guid userId);
    VechileInfoDTO? GetVechileKmPriceForUser(Guid userId);
    Task<Guid?> IsPaasengerAsync(Guid userId);
    string HashPassword(string password);
    Task<Guid?> GetUserIdByPassengerIdAsync(Guid passengerId);
    Task<Guid?> GetUserIdByDriverIdAsync(Guid driverId);
    Task<string?> GetStripeCustomerIdByUserIdAsync(Guid userId);
    Task<Result> UpdateStripeCustomerIdAsync(Guid userId, string stripeCustomerId);
    Task<Dictionary<Guid, Guid>> GetUserIdsByDriverIdsAsync(IEnumerable<Guid> driverIds);
}