using Lines.Application.Shared;

namespace Lines.Application.Features.TripRequests;

public static class TripRequestErrors
{
    public static Error TripRequestNotFound => new Error("TRIPREQUEST:NOTFOUND", "Trip request not found", ErrorType.Validation);
    public static Error TripRequestAcceptedOrCanceled => new Error("TRIPREQUEST:ACCEPTEDORCANCELED", "Trip request accepted or canceled", ErrorType.Validation);
    public static Error NotYourTripRequest => new Error("TRIPREQUEST:NotYourTripRequest", "You do not own this trip request", ErrorType.Validation);
    public static Error InvalidUserReward => new Error("TRIPREQUEST:InvalidUserReward", "User reward for this trip request is invalid", ErrorType.Validation);
    public static Error InvalidPrice(string description) => new Error("TRIPREQUEST:Invalid Price", description, ErrorType.Validation);
}