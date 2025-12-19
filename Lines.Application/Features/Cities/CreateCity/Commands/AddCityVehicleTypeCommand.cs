using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using MediatR;

namespace Lines.Application.Features.Cities.Commands;

public record AddCityVehicleTypeCommand(Guid CityId, Guid VehicleTypeId) : IRequest<bool>;
public class AddCityVehicleTypeCommandHandler : RequestHandlerBase<AddCityVehicleTypeCommand , bool>
{
    private readonly IRepository<CityVehicleType> _repository;
    public AddCityVehicleTypeCommandHandler(RequestHandlerBaseParameters parameters, IRepository<CityVehicleType> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<bool> Handle(AddCityVehicleTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new CityVehicleType(request.CityId, request.VehicleTypeId);
        await _repository.AddAsync(entity, cancellationToken);
        return true;
    }
}