using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;

public record RegisterDriverContactInfoOrchestrator(string RegistrationToken, RegisterDriverContactInfoDto ContactInfo) : IRequest<Result<bool>>;

public class RegisterDriverContactInfoOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<DriverRegistration> repository) 
    : RequestHandlerBase<RegisterDriverContactInfoOrchestrator, Result<bool>>(parameters)
{
    private readonly IRepository<DriverRegistration> _repository = repository;
    
    public override async Task<Result<bool>> Handle(RegisterDriverContactInfoOrchestrator request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing registration
            var registration = _repository.Get(r => r.RegistrationToken == request.RegistrationToken).FirstOrDefault();
            
            if (registration == null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Registration not found"));
            }

            if (registration.Status != RegistrationStatus.InProgress)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Registration is already completed"));
            }

            // Check for duplicate email
            var existingRegistration = _repository.Get(r => r.Email == request.ContactInfo.Email).FirstOrDefault();
            if (existingRegistration != null)
            {
                return Result<bool>.Failure(DriverErrors.RegisterDriverError("Email is already registered"));
            }

            // Update contact info
            registration.Email = request.ContactInfo.Email;
            registration.IsContactInfoCompleted = true;

            await _repository.UpdateAsync(registration, cancellationToken);
            _repository.SaveChanges();

            // Automatically send email verification
            var sendVerificationResult = await _mediator.Send(new SendEmailVerificationOrchestrator(request.RegistrationToken), cancellationToken);
            
            if (!sendVerificationResult.IsSuccess)
            {
                // Log the error but don't fail the contact info step
                // The user can manually request verification later
                // TODO: Add logging here
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(DriverErrors.RegisterDriverError($"Failed to process contact info: {ex.Message}"));
        }
    }
}