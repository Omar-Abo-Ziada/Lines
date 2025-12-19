using Application.Common.Helpers;
using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Domain.Models.Vehicles;
using LinqKit;

namespace Lines.Application.Features.VehicleTypes.Commands;

public record GetAllVehicleTypesQuery(string? Name , int? Capacity, int PageSize, int PageNumber) : IRequest<PagingDto<GetAllVehicleTypesDto>>;
public class GetAllVehicleTypesQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<VehicleType> repository)
    : RequestHandlerBase<GetAllVehicleTypesQuery, PagingDto<GetAllVehicleTypesDto>>(parameters)
{
    private readonly IRepository<VehicleType> _repository = repository;

    public override async Task<PagingDto<GetAllVehicleTypesDto>> Handle(GetAllVehicleTypesQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<VehicleType>(true);
        if (!string.IsNullOrEmpty(request.Name))
        {
            predicate = predicate.And(x => x.Name.Contains(request.Name));
        }

        if (request.Capacity != null)
        {
            predicate = predicate.And(x => x.Capacity == request.Capacity);
        }

        var query = _repository
                                                    .Get(predicate)
                                                    .ProjectToType<GetAllVehicleTypesDto>();
        var result = await PagingHelper.CreateAsync(query, request.PageSize, request.PageNumber, cancellationToken);
        return result;
    }
}