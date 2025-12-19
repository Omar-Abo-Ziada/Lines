using Lines.Application.Features.Drivers.GetDriverById.DTOs;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Drivers.GetDriverById.Queries
{
    public record GetDriverByIdQuery(Guid driverId) : IRequest<Driver?>;

    public class GetDriverByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> repository)
        : RequestHandlerBase<GetDriverByIdQuery, Driver?>(parameters)
    {
        public async override Task<Driver?> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
        {
            var driver = await repository.GetByIdAsync(request.driverId, cancellationToken);
            if (driver is null)
            {
                return null;
            }

            //var driverDto = driver.Adapt<GetDriverByIdDto>();

            //return driverDto;

            return driver;
        }
    }

}
