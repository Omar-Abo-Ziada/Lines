using Lines.Application.Shared;

namespace Lines.Application.Features.Passengers;

public static class PassengerErrors
{
    public static Error RegisterPassengerError(string desc) => new Error("PASSENGER.REGISTER_ERROR", desc, ErrorType.Validation);
    public static Error InvalidReferralCodeError() => new Error("PASSENGER.InvalidReferralCodeError", "There is no passenger found with this referral code", ErrorType.Validation);
    public static Error ReferralCodeAlreadySubmitted() => new Error("PASSENGER.ReferralCodeAlreadySubmitted", "You have already submitted a referral code before", ErrorType.Validation);
    public static Error CannotReferYourself() => new Error("PASSENGER.CannotReferYourself", "You can not use your own referral code", ErrorType.Validation);
} 