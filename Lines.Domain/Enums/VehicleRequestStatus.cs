using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Enums
{
    public enum VehicleRequestStatus
    {
        PendingVerification = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3
    }
}
