namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record EmailVerificationResponseDto(bool IsSuccess, string Message, bool IsEmailVerified = false);
