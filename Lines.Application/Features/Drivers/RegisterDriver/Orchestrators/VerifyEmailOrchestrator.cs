using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record VerifyEmailOrchestrator(string RegistrationToken, string VerificationCode) : IRequest<Result<EmailVerificationResponseDto>>;

public class VerifyEmailOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
    : RequestHandlerBase<VerifyEmailOrchestrator, Result<EmailVerificationResponseDto>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    
    public override async Task<Result<EmailVerificationResponseDto>> Handle(VerifyEmailOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing registration
            var registration = _repository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            if (string.IsNullOrWhiteSpace(registration.EmailVerificationCode))
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("No verification code found. Please request a new one."));
            }

            if (registration.EmailVerificationExpiry == null || registration.EmailVerificationExpiry <= DateTime.UtcNow)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Verification code has expired. Please request a new one."));
            }

            if (registration.EmailVerificationCode != request.VerificationCode)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Invalid verification code."));
            }

            // Mark email as verified
            registration.IsEmailVerified = true;
            registration.EmailVerificationCode = null; // Clear the code after successful verification
            registration.EmailVerificationExpiry = null;

            await _repository.UpdateAsync(registration, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result<EmailVerificationResponseDto>.Success(new EmailVerificationResponseDto(
                true, 
                "Email verified successfully", 
                true
            ));
        }
        catch (Exception ex)
        {
            return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError($"An error occurred: {ex.Message}"));
        }
    }
}
