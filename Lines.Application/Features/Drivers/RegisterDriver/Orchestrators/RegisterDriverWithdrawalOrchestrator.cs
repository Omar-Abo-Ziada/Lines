using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Payments.DriverStripeAccounts.Orchestrators;
using Lines.Application.Features.RewardActions.GetRewardActionByType.Queries;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.License;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Models.Sites;
using Lines.Domain.Models.Vehicles;
using Lines.Domain.Shared;
using Lines.Domain.Value_Objects;
using MediatR;
using System.Text.Json;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverWithdrawalOrchestrator(string RegistrationToken, RegisterDriverWithdrawalDto WithdrawalInfo) : IRequest<Result<Guid>>;

public class RegisterDriverWithdrawalOrchestratorHandler(RequestHandlerBaseParameters parameters, 
    IRepository<DriverRegistration> registrationRepository,
    IRepository<DriverAddress> addressRepository,
    IRepository<Driver> driverRepository,
    IRepository<BankAccount> bankAccountRepository,
    IRepository<Vehicle> vehicleRepository,
    IRepository<DriverLicense> licenseRepository,
    IRepository<LicensePhoto> licensePhotoRepository,
    IApplicationUserService applicationUserService) 
    : RequestHandlerBase<RegisterDriverWithdrawalOrchestrator, Result<Guid>>(parameters)
{
    private readonly IRepository<DriverRegistration> _registrationRepository = registrationRepository;
    private readonly IRepository<DriverAddress> _addressRepository = addressRepository;
    private readonly IRepository<Driver> _driverRepository = driverRepository;
    private readonly IRepository<BankAccount> _bankAccountRepository = bankAccountRepository;
    private readonly IRepository<Vehicle> _vehicleRepository = vehicleRepository;
    private readonly IRepository<DriverLicense> _licenseRepository = licenseRepository;
    private readonly IRepository<LicensePhoto> _licensePhotoRepository = licensePhotoRepository;
    private readonly IApplicationUserService _applicationUserService = applicationUserService;

    public override async Task<Result<Guid>> Handle(RegisterDriverWithdrawalOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing registration
            var registration = _registrationRepository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<Guid>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            if (registration.Status != RegistrationStatus.InProgress)
            {
                return Result<Guid>.Failure(DriverErrors.RegisterDriverError("Registration is already completed"));
            }

            // Verify all previous steps completed
            if (!registration.IsPersonalInfoCompleted || 
                !registration.IsContactInfoCompleted || 
                !registration.IsAddressCompleted || 
                !registration.IsLicenseCompleted || 
                !registration.IsVehicleCompleted)
            {
                return Result<Guid>.Failure(DriverErrors.RegisterDriverError("All previous steps must be completed"));
            }

            // Check email verification
            if (!registration.IsEmailVerified)
            {
                return Result<Guid>.Failure(DriverErrors.RegisterDriverError("Please verify your email before completing registration"));
            }

            // Store bank account data
            var bankAccountData = new
            {
                BankName = request.WithdrawalInfo.BankName,
                IBAN = request.WithdrawalInfo.IBAN,
                SWIFT = request.WithdrawalInfo.SWIFT,
                AccountHolderName = request.WithdrawalInfo.AccountHolderName
            };

            registration.BankAccountData = JsonSerializer.Serialize(bankAccountData);
            registration.IsWithdrawalInfoCompleted = true;

            // Get reward points for registration completion
            var rewardActionDto = await _mediator.Send(new GetRewardActionByTypeQuery(RewardActionType.CompleteProfile), cancellationToken);

            // Create Driver entity first
            var driverId = Guid.NewGuid();
            var driver = new Driver(
                driverId,
                registration.FirstName!,
                registration.LastName!,
                new Email(registration.Email),
                new PhoneNumber(registration.PhoneNumber!),
                isNotifiedForOnlyTripsAboveMyPrice: false
            );

            // Set additional driver properties
            driver.CompanyName = registration.CompanyName;
            driver.CommercialRegistration = registration.CommercialRegistration;
            driver.DateOfBirth = registration.DateOfBirth;
            driver.IdentityType = registration.IdentityType;
            driver.PersonalPictureUrl = registration.PersonalPictureUrl;
            driver.PersonalInfoCompleted = registration.IsPersonalInfoCompleted;
            driver.ContactInfoCompleted = registration.IsContactInfoCompleted;
            driver.AddressCompleted = registration.IsAddressCompleted;
            driver.LicenseCompleted = registration.IsLicenseCompleted;
            driver.VehicleCompleted = registration.IsVehicleCompleted;
            driver.WithdrawalCompleted = registration.IsWithdrawalInfoCompleted;
            driver.RegistrationStatus = RegistrationStatus.PendingReview;

            // Save Driver to database first
            await _driverRepository.AddAsync(driver, cancellationToken);
            _driverRepository.SaveChanges(); // Save early to ensure Driver exists before ApplicationUser creation

            // Now create ApplicationUser with reference to existing Driver
            var appUserResult = await _applicationUserService.RegisterDriverAsync(driver, registration.PasswordHash!, rewardActionDto.Points);
            
            if (!appUserResult.Succeeded)
            {
                return Result<Guid>.Failure(DriverErrors.RegisterDriverError($"Failed to create ApplicationUser: {string.Join(", ", appUserResult.Errors)}"));
            }

            // Create DriverAddress
            if (registration.CityId.HasValue)
            {
                var driverAddress = new DriverAddress(
                    driver.Id,
                    registration.CityId.Value,
                    registration.Address!,
                    registration.PostalCode!,
                    registration.SectorId,
                    registration.LimousineBadgeUrl,
                    true // IsPrimary
                );

                await _addressRepository.AddAsync(driverAddress, cancellationToken);
            }

            // Create BankAccount
            var bankAccount = new BankAccount(
                request.WithdrawalInfo.BankName,
                request.WithdrawalInfo.IBAN,
                request.WithdrawalInfo.SWIFT,
                request.WithdrawalInfo.AccountHolderName,
                driver.Id
            );

            await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);

            // Link bank account to driver
            driver.AddBankAccount(bankAccount);

            // Create Vehicle from VehicleData JSON
            if (!string.IsNullOrEmpty(registration.VehicleData))
            {
                try
                {
                    var vehicleData = JsonSerializer.Deserialize<VehicleDataDto>(registration.VehicleData);
                    
                    if (vehicleData != null)
                    {
                        var vehicle = new Vehicle(
                            driver.Id,
                            vehicleData.VehicleTypeId,
                            "", // Make - not provided in registration, using empty string
                            vehicleData.Model,
                            vehicleData.Year,
                            vehicleData.Color,
                            vehicleData.LicensePlate,
                            vehicleData.KmPrice
                        );
                        
                        // Set as primary vehicle (first vehicle)
                        vehicle.SetAsPrimary();
                        vehicle.Activate();
                        
                        // Store registration documents as JSON
                        if (vehicleData.DocumentUrls != null && vehicleData.DocumentUrls.Any())
                        {
                            vehicle.UpdateRegistrationDocuments(vehicleData.DocumentUrls);
                        }
                        
                        await _vehicleRepository.AddAsync(vehicle, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail registration
                    // Vehicle can be added later
                    // TODO: Add proper logging
                }
            }

            // Create DriverLicense from LicenseData JSON
            if (!string.IsNullOrEmpty(registration.LicenseData))
            {
                try
                {
                    var licenseData = JsonSerializer.Deserialize<LicenseDataDto>(registration.LicenseData);
                    
                    if (licenseData != null)
                    {
                        // Create LicensePhoto entities from URLs
                        var licensePhotos = new List<LicensePhoto>();
                        if (licenseData.LicensePhotoUrls != null && licenseData.LicensePhotoUrls.Any())
                        {
                            foreach (var photoUrl in licenseData.LicensePhotoUrls)
                            {
                                var licensePhoto = new LicensePhoto
                                {
                                    Id = Guid.NewGuid(),
                                    PhotoUrl = photoUrl,
                                    CreatedDate = DateTime.UtcNow
                                };
                                licensePhotos.Add(licensePhoto);
                                await _licensePhotoRepository.AddAsync(licensePhoto, cancellationToken);
                            }
                        }
                        
                        // Create DriverLicense entity
                        var driverLicense = new DriverLicense(
                            licenseData.LicenseNumber,
                            licenseData.IssueDate,
                            licenseData.ExpiryDate,
                            driver.Id,
                            licensePhotos,
                            true // IsCurrent - first license is current
                        );
                        
                        await _licenseRepository.AddAsync(driverLicense, cancellationToken);
                        
                        // Link license to driver
                        driver.AddLicense(driverLicense);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail registration
                    // License can be added later
                    // TODO: Add proper logging
                }
            }

            // Update registration status
            registration.Status = RegistrationStatus.PendingReview;
            registration.CompletedAt = DateTime.UtcNow;
            registration.CreatedDriverId = driver.Id;

            await _registrationRepository.UpdateAsync(registration, cancellationToken);
            
            // Save all pending changes across all repositories
            // Since all repositories share the same DbContext, calling SaveChanges on any repository saves all changes
            _registrationRepository.SaveChanges();


            //// ✅ Ensure Stripe Connected Account exists
            //await _mediator.Send(
            //    new CreateOrGetDriverStripeAccountOrchestrator(driver.Id),
            //    cancellationToken
            //);


            return Result<Guid>.Success(driver.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(DriverErrors.RegisterDriverError($"Failed to process withdrawal info: {ex.Message}"));
        }
    }
}