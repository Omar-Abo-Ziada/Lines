using Application.Common.Helpers;
using Lines.Application.Common;
using Lines.Application.Features.Trips.GetTrips.DTOs;
using Lines.Application.Shared;
using Lines.Domain.Enums;
using Lines.Application.Interfaces;
using Lines.Domain.Models.PaymentMethods;
using Lines.Domain.Models.Trips;
using LinqKit;
using MediatR;

namespace Lines.Application.Features.Trips.GetTrips.Queries;


public record GetTripsQuery(int? Status, int PageIndex = 1, int PageSize = 10) : IRequest<PagingDto<GetTripsDto>>;
public class GetTripsQueryHandler : RequestHandlerBase<GetTripsQuery, PagingDto<GetTripsDto>>
{
    private readonly IRepository<Trip> _repository;
    public GetTripsQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository) : base(parameters)
    {
        _repository = repository;
    }

    public override async Task<PagingDto<GetTripsDto>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Trip>(true);
        if (request.Status.HasValue)
        {
            predicate = predicate.And(x => x.Status ==  (TripStatus)request.Status.Value);
        }

        var query = _repository.Get(predicate)
            .Select(x => new GetTripsDto()
            {
                Id = x.Id,
                Status = x.Status,
                DriverName = $"{x.Driver.FirstName} {x.Driver.LastName}",
                PickUpDate = x.StartedAt,
                DropOffDate = x.EndedAt,
                PaymentMethod = x.PaymentMethodType,
                PaymentStatus = x.PaymentId.HasValue ? 1 : 0
            });
        
        var result = await PagingHelper.CreateAsync(query,request.PageIndex,  request.PageSize,  cancellationToken);
        return result;
    }
}