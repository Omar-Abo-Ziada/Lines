using Application.Common.Helpers;
using Lines.Application.Common;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Shared;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Vehicles;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record GetVehicleTypesListQuery() : IRequest<List<GetVehicleTypesListDto>>;
public class GetVehicleTypesListQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<GetVehicleTypesListQuery, List<GetVehicleTypesListDto>>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<List<GetVehicleTypesListDto>> Handle(GetVehicleTypesListQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository
            .Get(x => !x.IsDeleted)
            .ProjectToType<GetVehicleTypesListDto>()
            .ToListAsync(cancellationToken);
        return query;
    }
}