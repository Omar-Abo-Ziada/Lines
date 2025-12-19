using Lines.Application.Common;
using Lines.Application.Features.Trips.GetTripInvoice.DTOs;
using Lines.Application.Features.Trips.GetTripInvoice.Queries;
using Lines.Application.Features.Trips;
using Lines.Application.Interfaces;
using Lines.Application.Shared;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Trips.GetTripInvoice.Orchestrators;

public record GetTripInvoiceOrchestrator(Guid TripId) : IRequest<Result<TripInvoiceDto>>;

public class GetTripInvoiceOrchestratorHandler(RequestHandlerBaseParameters parameters, IRepository<Trip> repository)
    : RequestHandlerBase<GetTripInvoiceOrchestrator, Result<TripInvoiceDto>>(parameters)
{
    public async override Task<Result<TripInvoiceDto>> Handle(GetTripInvoiceOrchestrator request, CancellationToken cancellationToken)
    {
        // Validate trip exists
        var tripExists = await repository.Get()
            .AnyAsync(t => t.Id == request.TripId, cancellationToken);

        if (!tripExists)
        {
            return Result<TripInvoiceDto>.Failure(TripErrors.TripNotFound);
        }

        // Get invoice data
        var result = await _mediator.Send(new GetTripInvoiceQuery(request.TripId), cancellationToken);

        if (result == null)
        {
            return Result<TripInvoiceDto>.Failure(Error.NullValue);
        }

        return Result<TripInvoiceDto>.Success(result);
    }
}
