using Lines.Application.Features.Drivers.GetDriverContact.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverContact.Queries;

public record GetDriverContactQuery(Guid userId) : IRequest<DriverContactDto?>;

public class GetDriverContactQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> repository, IApplicationUserService applicationUserService)
    : RequestHandlerBase<GetDriverContactQuery, DriverContactDto?>(parameters)
{
    public async override Task<DriverContactDto?> Handle(GetDriverContactQuery request, CancellationToken cancellationToken)
    {
        // Get driver ID from user ID using ApplicationUserService
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(request.userId);
        if (userDriverIds?.DriverId == null)
        {
            return null; // User is not a driver
        }

        // Use projection to avoid EF Core owned entity materialization issues
        var driverContactDto = await repository.Get()
            .Where(d => d.Id == userDriverIds.DriverId.Value)
            .Select(d => new DriverContactDto
            {
                Email = d.Email.Value,
                PhoneNumber = d.PhoneNumber.Value
            })
            .FirstOrDefaultAsync(cancellationToken);

        return driverContactDto;
    }
}
