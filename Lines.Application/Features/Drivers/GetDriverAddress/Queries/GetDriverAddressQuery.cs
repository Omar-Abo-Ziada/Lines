using Lines.Application.Features.Drivers.GetDriverAddress.DTOs;
using Lines.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Lines.Application.Features.Drivers.GetDriverAddress.Queries;

public record GetDriverAddressQuery(Guid userId) : IRequest<DriverAddressDto?>;

public class GetDriverAddressQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Domain.Models.Drivers.Driver> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetDriverAddressQuery, DriverAddressDto?>(parameters)
{
    public async override Task<DriverAddressDto?> Handle(GetDriverAddressQuery request, CancellationToken cancellationToken)
    {
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null;
        }

        var driverAddressDto = await repository.Get()
            .Where(d => d.Id == userDriverIds.DriverId.Value)
            .SelectMany(d => d.Addresses.Where(a => a.IsPrimary))
            .Select(da => new DriverAddressDto
            {
                Address = da.Address,
                CityId = da.CityId,
                CityName = da.City.Name,
                SectorId = da.SectorId,
                SectorName = da.Sector != null ? da.Sector.Name : null,
                PostalCode = da.PostalCode
            })
            .FirstOrDefaultAsync(cancellationToken);

        return driverAddressDto;
    }
}
