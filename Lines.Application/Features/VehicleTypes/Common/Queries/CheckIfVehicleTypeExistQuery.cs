using Lines.Domain.Models.Vehicles;
using LinqKit;

namespace Lines.Application.Features.VehicleTypes.Common.Queries;

public record CheckIfVehicleTypeExistQuery(string Name, Guid? Id = null) :  IRequest<bool>;
public class CheckIfVehicleTypeExistQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<CheckIfVehicleTypeExistQuery, bool>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<bool> Handle(CheckIfVehicleTypeExistQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<VehicleType>(true);
        predicate = predicate.And(x => x.Name == request.Name.Trim());
        if (request.Id.HasValue)
        {
            predicate = predicate.And(x => x.Id != request.Id.Value);
        }
        return await _repository
            .AnyAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
    }
}