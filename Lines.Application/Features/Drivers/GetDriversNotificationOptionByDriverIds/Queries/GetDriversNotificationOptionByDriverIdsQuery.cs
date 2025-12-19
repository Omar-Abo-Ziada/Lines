using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetNearbyDriverConnectionsFilteredByKmPrice.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriversNotificationOptionByDriverIds.Queries
{
    public record GetDriversNotificationOptionByDriverIdsQuery(ICollection<Guid> DriverIds) 
                        : IRequest<ICollection<DriverNotificationOptionDto>>;
    
    public class GetDriversNotificationOptionByDriverIdsQueryHandler
        (RequestHandlerBaseParameters parameters, IRepository<Driver> repository)
            : RequestHandlerBase<GetDriversNotificationOptionByDriverIdsQuery, ICollection<DriverNotificationOptionDto>>(parameters)
    {
        public override async Task<ICollection<DriverNotificationOptionDto>> Handle(GetDriversNotificationOptionByDriverIdsQuery request, CancellationToken cancellationToken)
        {
            var driverOptions = await repository.SelectWhere(d => request.DriverIds.Contains(d.Id),
                                            d => new DriverNotificationOptionDto(d.Id, d.IsNotifiedForOnlyTripsAboveMyPrice))
                                            .ToListAsync();
            return driverOptions;
        }
    }


}
