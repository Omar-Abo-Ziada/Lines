using Lines.Application.Features.DriverBlockedPassengers.DTOs;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengers.Queries;

public record GetBlockedPassengersQuery(Guid DriverId) : IRequest<Result<List<BlockedPassengerDto>>>;

public class GetBlockedPassengersQueryHandler : RequestHandlerBase<GetBlockedPassengersQuery, Result<List<BlockedPassengerDto>>>
{
    private readonly IRepository<DriverBlockedPassenger> _repository;

    public GetBlockedPassengersQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverBlockedPassenger> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<List<BlockedPassengerDto>>> Handle(
        GetBlockedPassengersQuery request,
        CancellationToken cancellationToken)
    {
        var blockedPassengers = await _repository.Get()
            .Where(dbp => dbp.DriverId == request.DriverId && !dbp.IsDeleted)
            .Include(dbp => dbp.Passenger)
            .OrderByDescending(dbp => dbp.CreatedDate)
            .Select(dbp => new BlockedPassengerDto
            {
                Id = dbp.Id,
                PassengerId = dbp.PassengerId,
                FirstName = dbp.Passenger.FirstName,
                LastName = dbp.Passenger.LastName,
                Email = dbp.Passenger.Email.Value,
                PhoneNumber = dbp.Passenger.PhoneNumber.Value,
                Rating = dbp.Passenger.Rating,
                TotalTrips = dbp.Passenger.TotalTrips,
                Reason = dbp.Reason,
                BlockedDate = dbp.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return Result<List<BlockedPassengerDto>>.Success(blockedPassengers);
    }
}

