using Lines.Domain.Models.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Offers.GetOfferByRequestId.DTOs
{
    public class GetOfferByRequestIdDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TimeToArriveInMinutes { get; set; }
        public float DistanceToArriveInMeters { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterRewardApplied { get; set; }
        public Guid DriverId { get; set; }
        public bool IsAccepted { get; set; }
        public Guid TripRequestId { get; set; }

    }
}
