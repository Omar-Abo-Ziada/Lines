using Lines.Domain.Models.Users;
using LinqKit;

namespace Lines.Application.Features.EmergencyNumbers;

public record CheckIfEmergencyNumberExist(string PhoneNumber, EmergencyNumberType EmergencyNumberType , Guid? UserId) : IRequest<bool>;

public class CheckIfEmergencyNumberExistHandler(RequestHandlerBaseParameters parameters,IRepository<EmergencyNumber> repository)
    : RequestHandlerBase<CheckIfEmergencyNumberExist, bool>(parameters)
{
    private readonly IRepository<EmergencyNumber> _repository = repository;

    public override async Task<bool> Handle(CheckIfEmergencyNumberExist request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<EmergencyNumber>();
        predicate = predicate.And(x => x.PhoneNumber == request.PhoneNumber);
        predicate = predicate.And(x => x.EmergencyNumberType == request.EmergencyNumberType);
        if (request.UserId.HasValue)
        {
            predicate = predicate.And(x => x.UserId == request.UserId.Value);
        }
        return await _repository.AnyAsync(predicate, cancellationToken);
    }
}