using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverBlockedPassengers.UnblockPassenger.Commands;

public record UnblockPassengerCommand(Guid DriverId, Guid PassengerId) : IRequest<Result<bool>>;

public class UnblockPassengerCommandHandler : RequestHandlerBase<UnblockPassengerCommand, Result<bool>>
{
    private readonly IRepository<DriverBlockedPassenger> _repository;

    public UnblockPassengerCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverBlockedPassenger> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<bool>> Handle(
        UnblockPassengerCommand request,
        CancellationToken cancellationToken)
    {
        var blockedPassenger = await _repository.Get()
            .FirstOrDefaultAsync(dbp => 
                dbp.DriverId == request.DriverId && 
                dbp.PassengerId == request.PassengerId &&
                !dbp.IsDeleted,
                cancellationToken);

        if (blockedPassenger == null)
        {
            return Result<bool>.Failure(
                new Error("PASSENGER_NOT_BLOCKED", "This passenger is not blocked", ErrorType.NotFound));
        }

        blockedPassenger.delete();
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}

