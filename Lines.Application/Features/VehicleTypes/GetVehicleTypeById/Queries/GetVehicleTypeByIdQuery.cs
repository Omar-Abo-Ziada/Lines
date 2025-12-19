using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record GetVehicleTypeByIdQuery(Guid Id) : IRequest<GetVehicleTypeByIdDto>;
public class GetVehicleTypeByIdQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<GetVehicleTypeByIdQuery, GetVehicleTypeByIdDto>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<GetVehicleTypeByIdDto?> Handle(GetVehicleTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository
                .Get(x => x.Id == request.Id)
                .ProjectToType<GetVehicleTypeByIdDto>()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }
}