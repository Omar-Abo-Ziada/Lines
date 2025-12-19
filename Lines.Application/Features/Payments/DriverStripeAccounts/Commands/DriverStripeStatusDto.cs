using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Payments.DriverStripeAccounts.Commands
{
    public record DriverStripeStatusDto(
      bool ChargesEnabled,
      bool PayoutsEnabled,
      bool DetailsSubmitted
  );

}
