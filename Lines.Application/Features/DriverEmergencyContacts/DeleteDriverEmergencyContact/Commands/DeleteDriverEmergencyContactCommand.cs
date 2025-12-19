using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverEmergencyContacts.DeleteDriverEmergencyContact.Commands;

public record DeleteDriverEmergencyContactCommand(Guid Id, Guid DriverId) 
    : IRequest<Result<bool>>;

public class DeleteDriverEmergencyContactCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<DriverEmergencyContact> repository)
    : RequestHandlerBase<DeleteDriverEmergencyContactCommand, Result<bool>>(parameters)
{
    public override async Task<Result<bool>> Handle(
        DeleteDriverEmergencyContactCommand request,
        CancellationToken cancellationToken)
    {
        var emergencyContact = await repository.Get()
            .FirstOrDefaultAsync(dec => 
                dec.Id == request.Id && 
                dec.DriverId == request.DriverId &&
                !dec.IsDeleted,
                cancellationToken);

        if (emergencyContact == null)
        {
            return Result<bool>.Failure(
                new Error("NOT_FOUND", "Emergency contact not found", ErrorType.NotFound));
        }

        emergencyContact.delete();
        await repository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}

