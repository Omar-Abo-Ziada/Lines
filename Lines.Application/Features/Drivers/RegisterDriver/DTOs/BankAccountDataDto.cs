namespace Lines.Application.Features.Drivers.RegisterDriver.DTOs;

public record BankAccountDataDto(
    string BankName,
    string IBAN,
    string SWIFT,
    string AccountHolderName
);
