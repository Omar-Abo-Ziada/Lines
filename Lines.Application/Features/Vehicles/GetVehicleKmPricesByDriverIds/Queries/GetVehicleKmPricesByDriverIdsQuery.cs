using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetNearbyDriverConnectionsFilteredByKmPrice.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.GetVehicleKmPricesByDriverIds.Queries
{
    public record GetVehicleKmPricesByDriverIdsQuery(ICollection<Guid> driverIds) : IRequest<ICollection<VehicleKmPriceDto>>;

    public class GetVehicleKmPricesByDriverIdsQueryhandler
        (RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository)
            : RequestHandlerBase<GetVehicleKmPricesByDriverIdsQuery, ICollection<VehicleKmPriceDto>>(parameters)
    {
        public override async Task<ICollection<VehicleKmPriceDto>> Handle(GetVehicleKmPricesByDriverIdsQuery request, CancellationToken cancellationToken)
        {
            var vehicleKmPrices = await repository.SelectWhere(v => request.driverIds.Contains(v.DriverId),
                                                           v => new VehicleKmPriceDto(v.DriverId, v.KmPrice))
                                            .ToListAsync(cancellationToken);
            return vehicleKmPrices;
        }
    }

}
