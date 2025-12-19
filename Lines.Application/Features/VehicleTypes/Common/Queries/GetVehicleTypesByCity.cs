using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.VehicleTypes.Common.Queries;

public record GetVehicleTypesByCity(Guid CityId) : IRequest<List<Guid>>;
public class GetVehicleTypesByCityHandler : RequestHandlerBase<GetVehicleTypesByCity, List<Guid>>
{
    private readonly IRepository<CityVehicleType> _repository;
    public GetVehicleTypesByCityHandler(RequestHandlerBaseParameters parameters, IRepository<CityVehicleType> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<List<Guid>> Handle(GetVehicleTypesByCity request, CancellationToken cancellationToken)
    {
        return await _repository.Get(x => x.CityId == request.CityId)
                .Select(x => x.VehicleTypeId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        
    }
}