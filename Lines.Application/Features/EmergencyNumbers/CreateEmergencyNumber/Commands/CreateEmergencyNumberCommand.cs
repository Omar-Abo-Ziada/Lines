using Lines.Domain.Models.Users;

namespace Lines.Application.Features.EmergencyNumbers;

public record CreateEmergencyNumberCommand(string Name, string PhoneNumber, EmergencyNumberType EmergencyNumberType, Guid? UserId) : IRequest<CreateEmergencyNumberDto>;
public class CreateEmergencyNumberCommandHandler(RequestHandlerBaseParameters parameters,IRepository<EmergencyNumber> repository)
    : RequestHandlerBase<CreateEmergencyNumberCommand, CreateEmergencyNumberDto>(parameters)
{
    private readonly IRepository<EmergencyNumber> _repository = repository;
    
    public async override Task<CreateEmergencyNumberDto> Handle(CreateEmergencyNumberCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = new PhoneNumber(request.PhoneNumber);
        var entity = new EmergencyNumber(request.Name, phoneNumber, request.EmergencyNumberType,request.UserId);
        
        var result = await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        return _mapper.Map<CreateEmergencyNumberDto>(result);
    }
}