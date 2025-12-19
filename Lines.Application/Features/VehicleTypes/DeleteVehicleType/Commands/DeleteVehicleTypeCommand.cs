using Lines.Domain.Models.Vehicles;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record DeleteVehicleTypeCommand(Guid Id) 
    : IRequest<bool>;
public class DeleteVehicleTypeCommandHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<DeleteVehicleTypeCommand, bool>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<bool> Handle(DeleteVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        return true;
    }
}