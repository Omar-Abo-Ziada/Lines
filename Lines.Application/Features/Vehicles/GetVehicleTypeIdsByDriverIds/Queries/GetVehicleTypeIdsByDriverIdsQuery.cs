using Lines.Application.Common;
using Lines.Application.Features.Vehicles.GetVehicleTypeIdByDriverId.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Vehicles.GetVehicleTypeIdByDriverId.Queries
{
    public record GetVehicleTypeIdsByDriverIdsQuery(ICollection<Guid> driverIds) : IRequest<ICollection<VehicleTypeIdDto>>;

    public class GetVehicleTypeIdsByDriverIdsQueryHandler
        (RequestHandlerBaseParameters parameters, IRepository<Vehicle> repository)
            : RequestHandlerBase<GetVehicleTypeIdsByDriverIdsQuery, ICollection<VehicleTypeIdDto>>(parameters)
    {
        public override async Task<ICollection<VehicleTypeIdDto>> Handle(GetVehicleTypeIdsByDriverIdsQuery request, CancellationToken cancellationToken)
        {
            var vehicleTypeIds = await repository.SelectWhere(v => request.driverIds.Contains(v.DriverId),
                                                           v => new VehicleTypeIdDto(v.DriverId, v.VehicleTypeId))
                                            .ToListAsync(cancellationToken);
            return vehicleTypeIds;
        }
    }
}
