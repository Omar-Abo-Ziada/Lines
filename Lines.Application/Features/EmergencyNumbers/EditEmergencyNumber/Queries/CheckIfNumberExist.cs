using Lines.Domain.Models.Users;
using LinqKit;

namespace Lines.Application.Features.EmergencyNumbers;

public record CheckIfNumberExist(string PhoneNumber, EmergencyNumberType EmergencyNumberType,Guid? UserId ) : IRequest<bool>;

public class CheckIfNumberExistHandler(RequestHandlerBaseParameters parameters,IRepository<EmergencyNumber> repository)
    : RequestHandlerBase<CheckIfNumberExist, bool>(parameters)
{
    private readonly IRepository<EmergencyNumber> _repository = repository;

    public override async Task<bool> Handle(CheckIfNumberExist request, CancellationToken cancellationToken)
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