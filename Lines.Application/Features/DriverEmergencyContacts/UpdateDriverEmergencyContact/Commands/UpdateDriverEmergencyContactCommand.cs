using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.Commands;

public record UpdateDriverEmergencyContactCommand(
    Guid Id,
    Guid DriverId,
    string Name,
    string PhoneNumber,
    DriverEmergencyContact EmergencyContactFromDb) 
    : IRequest<UpdateDriverEmergencyContactDto>;

public class UpdateDriverEmergencyContactCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<DriverEmergencyContact> repository)
    : RequestHandlerBase<UpdateDriverEmergencyContactCommand, UpdateDriverEmergencyContactDto>(parameters)
{
    public override async Task<UpdateDriverEmergencyContactDto> Handle(
        UpdateDriverEmergencyContactCommand request,
        CancellationToken cancellationToken)
    {
        var phoneNumber = new PhoneNumber(request.PhoneNumber);
        request.EmergencyContactFromDb.UpdateName(request.Name);
        request.EmergencyContactFromDb.UpdatePhoneNumber(phoneNumber);

        await repository.UpdateAsync(request.EmergencyContactFromDb, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<UpdateDriverEmergencyContactDto>(request.EmergencyContactFromDb);
    }
}

