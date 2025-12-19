using Lines.Application.Shared;

namespace Lines.Application.Features.Trips
{
    public static class TripErrors
    {
        public static Error InvalidStatus => new Error("Trip:Invalid status", "Only in progress trips can be marked as completed", ErrorType.Validation);
        public static Error PassengerMismatch => new Error("Trip:Passenger Mismatch", "Only the owner of the trip can be mark it as completed", ErrorType.Validation);
        public static Error TripNotFound => new Error("TRIP.NOT_FOUND", "Trip not found", ErrorType.NotFound);
        public static Error PaymentAlreadyCollected => new Error("TRIP.PAYMENT_ALREADY_COLLECTED", "Payment has already been collected for this trip", ErrorType.Validation);
        public static Error UnauthorizedTripAccess => new Error("TRIP.UNAUTHORIZED_ACCESS", "You do not have access to this trip", ErrorType.Validation);
    }
}
