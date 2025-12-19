using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Enums 
{
    public enum NotificationType
    {
        Welcoming = 0,
        TripRequestAccepted = 1,
        DriverOfferAccepted = 2,
        PaymentReceived = 3,
        TripRequestCancelled = 4,
        TripRequestCreated=5,
        DriverOfferCreated=6,
        PaymentSuccess,
        DriverEarning,
    }
}
