using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetDriverTrips.DTOs;
using Lines.Application.Features.Drivers.GetDriverTrips.Queries;
using Lines.Application.Features.Drivers;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverTrips.Orchestrators;

public record GetDriverTripsOrchestrator(
    Guid DriverId,
    int? TripStatus,
    DateTime? DateRangeStart,
    DateTime? DateRangeEnd,
    int? PaymentStatus,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<PagingDto<DriverTripDto>>>;

public class GetDriverTripsOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> driverRepository)
    : RequestHandlerBase<GetDriverTripsOrchestrator, Result<PagingDto<DriverTripDto>>>(parameters)
{
    public async override Task<Result<PagingDto<DriverTripDto>>> Handle(GetDriverTripsOrchestrator request, CancellationToken cancellationToken)
    {
        // Validate driver exists
        var driverExists = await driverRepository.Get()
            .AnyAsync(d => d.Id == request.DriverId, cancellationToken);

        if (!driverExists)
        {
            return Result<PagingDto<DriverTripDto>>.Failure(DriverErrors.DriverNotFound);
        }

        // Get trips
        var result = await _mediator.Send(new GetDriverTripsQuery(
            request.DriverId,
            request.TripStatus,
            request.DateRangeStart,
            request.DateRangeEnd,
            request.PaymentStatus,
            request.PageNumber,
            request.PageSize
        ), cancellationToken);

        return Result<PagingDto<DriverTripDto>>.Success(result);
    }
}
