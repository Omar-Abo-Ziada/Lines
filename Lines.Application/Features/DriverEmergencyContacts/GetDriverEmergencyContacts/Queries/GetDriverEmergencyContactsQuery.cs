using Lines.Application.Features.DriverEmergencyContacts.Shared.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverEmergencyContacts.GetDriverEmergencyContacts.Queries;

public record GetDriverEmergencyContactsQuery(Guid DriverId) : IRequest<List<DriverEmergencyContactDto>>;

public class GetDriverEmergencyContactsQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<DriverEmergencyContact> repository)
    : RequestHandlerBase<GetDriverEmergencyContactsQuery, List<DriverEmergencyContactDto>>(parameters)
{
    public override async Task<List<DriverEmergencyContactDto>> Handle(
        GetDriverEmergencyContactsQuery request,
        CancellationToken cancellationToken)
    {
        var contacts = await repository.Get()
            .Where(dec => dec.DriverId == request.DriverId)
            .ProjectToType<DriverEmergencyContactDto>()
            .ToListAsync(cancellationToken);

        return contacts;
    }
}

