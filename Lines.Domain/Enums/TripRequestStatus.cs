namespace Lines.Domain.Enums
{
    public enum TripRequestStatus
        {
            Pending,            // Request made by passenger, awaiting driver acceptance
            Accepted,           // A driver has accepted the request
            CancelledByPassenger
        }
}
