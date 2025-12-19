using Lines.Domain.Models.Users;

namespace Lines.Application.Features.EmergencyNumbers;

public record EditEmergencyNumberCommand(Guid Id ,string Name, string PhoneNumber, EmergencyNumberType EmergencyNumberType,
                                         EmergencyNumber emergencyNumberFromDb) : IRequest<EditEmergencyNumberDto>;
public class EditEmergencyNumberCommandHandler(RequestHandlerBaseParameters parameters,IRepository<EmergencyNumber> repository)
    : RequestHandlerBase<EditEmergencyNumberCommand, EditEmergencyNumberDto>(parameters)
{
    private readonly IRepository<EmergencyNumber> _repository = repository;
    
    public async override Task<EditEmergencyNumberDto> Handle(EditEmergencyNumberCommand request, CancellationToken cancellationToken)
    {

        request.emergencyNumberFromDb.PhoneNumber = new PhoneNumber(request.PhoneNumber);
        request.emergencyNumberFromDb.Name= request.Name;
        request.emergencyNumberFromDb.EmergencyNumberType= request.EmergencyNumberType;

        await _repository.UpdateAsync(request.emergencyNumberFromDb);

        _repository.SaveChanges();
                
        return _mapper.Map<EditEmergencyNumberDto>(request.emergencyNumberFromDb);
    }
}