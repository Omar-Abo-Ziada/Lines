using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverBlockedPassengers.BlockPassenger.Commands;

public record BlockPassengerCommand(Guid DriverId, Guid PassengerId, string? Reason) : IRequest<Result<Guid>>;

public class BlockPassengerCommandHandler : RequestHandlerBase<BlockPassengerCommand, Result<Guid>>
{
    private readonly IRepository<DriverBlockedPassenger> _repository;

    public BlockPassengerCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverBlockedPassenger> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<Result<Guid>> Handle(
        BlockPassengerCommand request,
        CancellationToken cancellationToken)
    {
        // Check if the block already exists
        var existingBlock = await _repository.Get()
            .FirstOrDefaultAsync(dbp => 
                dbp.DriverId == request.DriverId && 
                dbp.PassengerId == request.PassengerId &&
                !dbp.IsDeleted,
                cancellationToken);

        if (existingBlock != null)
        {
            return Result<Guid>.Failure(
                new Error("PASSENGER_ALREADY_BLOCKED", "This passenger is already blocked", ErrorType.Validation));
        }

        try
        {
            var blockedPassenger = new DriverBlockedPassenger(
                request.DriverId,
                request.PassengerId,
                request.Reason);

            await _repository.AddAsync(blockedPassenger, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(blockedPassenger.Id);
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Failure(
                new Error("INVALID_BLOCK_REQUEST", ex.Message, ErrorType.Validation));
        }
    }
}

