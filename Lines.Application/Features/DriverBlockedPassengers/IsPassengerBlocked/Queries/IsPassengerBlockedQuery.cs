using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverBlockedPassengers.IsPassengerBlocked.Queries;

public record IsPassengerBlockedQuery(Guid DriverId, Guid PassengerId) : IRequest<bool>;

public class IsPassengerBlockedQueryHandler : RequestHandlerBase<IsPassengerBlockedQuery, bool>
{
    private readonly IRepository<DriverBlockedPassenger> _repository;

    public IsPassengerBlockedQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverBlockedPassenger> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<bool> Handle(
        IsPassengerBlockedQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.Get()
            .AnyAsync(dbp => 
                dbp.DriverId == request.DriverId && 
                dbp.PassengerId == request.PassengerId && 
                !dbp.IsDeleted,
                cancellationToken);
    }
}

