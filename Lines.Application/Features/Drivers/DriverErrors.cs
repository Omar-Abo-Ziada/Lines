using Lines.Application.Shared;

namespace Lines.Application.Features.Drivers;

public static class DriverErrors
{
    public static Error RegisterDriverError(string desc) => new Error("DRIVER.REGISTER_ERROR",desc,ErrorType.Validation);
    public static Error DriverNotFound => new Error("DRIVER.NOT_FOUND", "Driver not found", ErrorType.NotFound);
    public static Error BankAccountNotFound => new Error("BANK_ACCOUNT.NOT_FOUND", "Bank account not found", ErrorType.NotFound);
    public static Error BankAccountAccessDenied => new Error("BANK_ACCOUNT.ACCESS_DENIED", "Access denied. Bank account does not belong to this driver", ErrorType.Validation);
}