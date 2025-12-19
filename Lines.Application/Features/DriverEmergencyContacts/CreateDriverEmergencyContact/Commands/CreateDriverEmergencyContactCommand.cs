using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Value_Objects;

namespace Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.Commands;

public record CreateDriverEmergencyContactCommand(Guid DriverId, string Name, string PhoneNumber) 
    : IRequest<CreateDriverEmergencyContactDto>;

public class CreateDriverEmergencyContactCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<DriverEmergencyContact> repository)
    : RequestHandlerBase<CreateDriverEmergencyContactCommand, CreateDriverEmergencyContactDto>(parameters)
{
    public override async Task<CreateDriverEmergencyContactDto> Handle(
        CreateDriverEmergencyContactCommand request,
        CancellationToken cancellationToken)
    {
        var phoneNumber = new PhoneNumber(request.PhoneNumber);
        var entity = new DriverEmergencyContact(request.DriverId, request.Name, phoneNumber);
        
        var result = await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<CreateDriverEmergencyContactDto>(result);
    }
}

