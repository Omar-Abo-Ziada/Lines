using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Common.Commands;
using Lines.Application.Interfaces;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverPersonalInfoOrchestrator(RegisterDriverPersonalInfoDto PersonalInfo) : IRequest<Result<string>>;

public class RegisterDriverPersonalInfoOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository, IApplicationUserService userService) 
    : RequestHandlerBase<RegisterDriverPersonalInfoOrchestrator, Result<string>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    private readonly IApplicationUserService _userService = userService;
    
    public override async Task<Result<string>> Handle(RegisterDriverPersonalInfoOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Check for duplicate phone number
            var existingRegistration = _repository.Get(r => r.PhoneNumber == request.PersonalInfo.PhoneNumber).FirstOrDefault();
            if (existingRegistration != null)
            {
                return Result<string>.Failure(DriverErrors.RegisterDriverError("Phone number is already registered"));
            }

            // Upload personal picture if provided
            string? personalPictureUrl = null;
            if (request.PersonalInfo.PersonalPicture != null)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(request.PersonalInfo.PersonalPicture, "driver-photos"), cancellationToken);
                if (!uploadResult.IsSuccess)
                {
                    return Result<string>.Failure(uploadResult.Error);
                }
                personalPictureUrl = uploadResult.Value;
            }

            // Hash password using the user service
            var passwordHash = _userService.HashPassword(request.PersonalInfo.Password);

            // Generate registration token
            var registrationToken = Guid.NewGuid().ToString();

            // Create DriverRegistration record
            var driverRegistration = new DriverRegistration
            {
                RegistrationToken = registrationToken,
                PhoneNumber = request.PersonalInfo.PhoneNumber, // Store phone as initial identifier
                Status = RegistrationStatus.InProgress,
                
                // Step 1: Personal Info
                PersonalPictureUrl = personalPictureUrl,
                FirstName = request.PersonalInfo.FirstName,
                LastName = request.PersonalInfo.LastName,
                CompanyName = request.PersonalInfo.CompanyName,
                CommercialRegistration = request.PersonalInfo.CommercialRegistration,
                DateOfBirth = request.PersonalInfo.DateOfBirth,
                PasswordHash = passwordHash,
                IdentityType = request.PersonalInfo.IdentityType,
                
                // Mark step 1 as completed
                IsPersonalInfoCompleted = true
            };

            await _repository.AddAsync(driverRegistration, cancellationToken);
            _repository.SaveChanges();

            return Result<string>.Success(registrationToken);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(DriverErrors.RegisterDriverError($"Failed to process personal info: {ex.Message}"));
        }
    }
}
