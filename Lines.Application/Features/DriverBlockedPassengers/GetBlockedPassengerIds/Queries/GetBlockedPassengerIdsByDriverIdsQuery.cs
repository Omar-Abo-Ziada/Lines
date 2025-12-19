using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengerIds.Queries;

/// <summary>
/// Gets a dictionary mapping driver IDs to their list of blocked passenger IDs.
/// This is used for efficient filtering in trip request matching.
/// </summary>
public record GetBlockedPassengerIdsByDriverIdsQuery(List<Guid> DriverIds, Guid PassengerId) : IRequest<List<Guid>>;

public class GetBlockedPassengerIdsByDriverIdsQueryHandler : RequestHandlerBase<GetBlockedPassengerIdsByDriverIdsQuery, List<Guid>>
{
    private readonly IRepository<DriverBlockedPassenger> _repository;

    public GetBlockedPassengerIdsByDriverIdsQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverBlockedPassenger> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<List<Guid>> Handle(
        GetBlockedPassengerIdsByDriverIdsQuery request,
        CancellationToken cancellationToken)
    {
        // Get all driver IDs who have blocked the specified passenger
        var driverIdsWhoBlockedPassenger = await _repository.Get()
            .Where(dbp => 
                request.DriverIds.Contains(dbp.DriverId) &&
                dbp.PassengerId == request.PassengerId &&
                !dbp.IsDeleted)
            .Select(dbp => dbp.DriverId)
            .ToListAsync(cancellationToken);

        return driverIdsWhoBlockedPassenger;
    }
}

