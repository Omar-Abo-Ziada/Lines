namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record VerifyEmailDto(string RegistrationToken, string VerificationCode);
