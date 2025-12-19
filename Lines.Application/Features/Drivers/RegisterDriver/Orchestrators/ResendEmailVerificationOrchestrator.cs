using Lines.Application.Common.Email;
using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record ResendEmailVerificationOrchestrator(string RegistrationToken) : IRequest<Result<EmailVerificationResponseDto>>;

public class ResendEmailVerificationOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository, IEmailService emailService) 
    : RequestHandlerBase<ResendEmailVerificationOrchestrator, Result<EmailVerificationResponseDto>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    private readonly IEmailService _emailService = emailService;
    
    public override async Task<Result<EmailVerificationResponseDto>> Handle(ResendEmailVerificationOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing registration
            var registration = _repository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            if (string.IsNullOrWhiteSpace(registration.Email))
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Email is required for verification"));
            }

            if (registration.IsEmailVerified)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Email is already verified"));
            }

            // Generate new 6-digit OTP
            var otpCode = GenerateOTP();
            var expiryTime = DateTime.UtcNow.AddMinutes(10);

            // Update registration with new verification code
            registration.EmailVerificationCode = otpCode;
            registration.EmailVerificationExpiry = expiryTime;

            await _repository.UpdateAsync(registration, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            // Send verification email
            var fullName = $"{registration.FirstName} {registration.LastName}";
            var mailData = new MailData(
                registration.Email, 
                fullName, 
                MailSubjects.DriverEmailVerification,
                string.Format(MailFormat.DriverEmailVerification.Html, fullName, otpCode)
            );

            var sendResult = await _emailService.SendMailAsync(mailData);
            
            if (!sendResult.Succeeded)
            {
                return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError("Failed to send verification email"));
            }

            return Result<EmailVerificationResponseDto>.Success(new EmailVerificationResponseDto(
                true, 
                "New verification code sent to your email", 
                false
            ));
        }
        catch (Exception ex)
        {
            return Result<EmailVerificationResponseDto>.Failure(DriverErrors.RegisterDriverError($"An error occurred: {ex.Message}"));
        }
    }

    private static string GenerateOTP()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
