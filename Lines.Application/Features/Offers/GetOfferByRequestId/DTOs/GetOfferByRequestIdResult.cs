using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Offers.GetOfferByRequestId.DTOs
{
    public class GetOfferByRequestIdResult
    {
        public List<GetOfferByRequestIdDTO> Data { get; set; }

        public int Total { get; set; }

    }
}
