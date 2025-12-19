using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Drivers.GetNearbyDriverConnectionsFilteredByKmPrice.DTOs
{
    public class DriverNotificationOptionDto
    {
        public Guid DriverId { get; set; }
        public bool IsNotifiedForOnlyTripsAboveMyPrice { get; set; }

        public DriverNotificationOptionDto(Guid driverId , bool isNotifiedForOnlyTripsAboveMyPrice)
        {
            DriverId = driverId;
            IsNotifiedForOnlyTripsAboveMyPrice = isNotifiedForOnlyTripsAboveMyPrice;
        }
    }
}
