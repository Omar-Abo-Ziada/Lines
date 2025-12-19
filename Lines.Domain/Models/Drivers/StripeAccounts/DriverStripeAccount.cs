using Lines.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Domain.Models.Drivers.StripeAccounts
{
    public class DriverStripeAccount : BaseModel
    {
        public Guid DriverId { get; private set; }
        public string StripeAccountId { get; private set; } = default!;
        public bool ChargesEnabled { get; private set; }
        public bool PayoutsEnabled { get; private set; }
        public bool DetailsSubmitted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public DriverStripeAccount() { }

        public DriverStripeAccount(Guid driverId, string stripeAccountId)
        {
            DriverId = driverId;
            StripeAccountId = stripeAccountId;
            ChargesEnabled = false;
            PayoutsEnabled = false;
            DetailsSubmitted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(bool chargesEnabled, bool payoutsEnabled, bool detailsSubmitted)
        {
            ChargesEnabled = chargesEnabled;
            PayoutsEnabled = payoutsEnabled;
            DetailsSubmitted = detailsSubmitted;
        }
    }

}
