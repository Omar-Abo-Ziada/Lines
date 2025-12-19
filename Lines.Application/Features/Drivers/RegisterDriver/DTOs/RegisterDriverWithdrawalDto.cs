namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record RegisterDriverWithdrawalDto(
    string BankName,
    string IBAN,
    string SWIFT,
    string AccountHolderName
);
