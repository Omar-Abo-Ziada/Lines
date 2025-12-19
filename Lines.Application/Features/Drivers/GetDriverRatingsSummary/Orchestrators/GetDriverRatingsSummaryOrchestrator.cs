using Lines.Application.Common;
using Lines.Application.Features.Drivers.GetDriverRatingsSummary.DTOs;
using Lines.Application.Features.Drivers.GetDriverRatingsSummary.Queries;
using Lines.Application.Features.Drivers;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Drivers.GetDriverRatingsSummary.Orchestrators;

public record GetDriverRatingsSummaryOrchestrator(Guid DriverId) : IRequest<Result<DriverRatingsSummaryDto>>;

public class GetDriverRatingsSummaryOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Driver> driverRepository)
    : RequestHandlerBase<GetDriverRatingsSummaryOrchestrator, Result<DriverRatingsSummaryDto>>(parameters)
{
    public async override Task<Result<DriverRatingsSummaryDto>> Handle(GetDriverRatingsSummaryOrchestrator request, CancellationToken cancellationToken)
    {
        //// Validate driver exists
        //var driverExists = await driverRepository.Get()
        //    .AnyAsync(d => d.Id == request.DriverId, cancellationToken);

        //if (!driverExists)
        //{
        //    return Result<DriverRatingsSummaryDto>.Failure(DriverErrors.DriverNotFound);
        //}

        // Get ratings summary
        var result = await _mediator.Send(new GetDriverRatingsSummaryQuery(request.DriverId), cancellationToken);

        if (result == null)
        {
            return Result<DriverRatingsSummaryDto>.Failure(Error.NullValue);
        }

        return Result<DriverRatingsSummaryDto>.Success(result);
    }
}
