using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Wallets.DTOs
{
    public class CreateWalletTopUpPaymentIntentDto
    {
        public string PaymentIntentId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        //public long AmountInMinorUnit { get; set; }       // زي trip
        public string Currency { get; set; } = null!;
    }

}
