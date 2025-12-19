using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Wallets.DTOs
{
    public class PayTripWithWalletDto
    {
        public Guid TripId { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; } = null!;
    }
}
