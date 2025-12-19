using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;
//using System.Data.Entity;

namespace Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByTripId.Queries
{
    public record GetEndTripLocationIdsByTripRequestIdQuery(Guid TripRequestId) : IRequest<List<Guid>>;

    public class GetEndTripLocationIdsByTripRequestIdQueryHandler(
               RequestHandlerBaseParameters parameters, IRepository<EndTripLocation> endTripLocationRepository)
        : RequestHandlerBase<GetEndTripLocationIdsByTripRequestIdQuery, List<Guid>>(parameters)
    {
        public override async Task<List<Guid>> Handle(GetEndTripLocationIdsByTripRequestIdQuery request, CancellationToken cancellationToken)
        {
            var endTripLocationIds = await endTripLocationRepository.Get(el => el.TripRequestId == request.TripRequestId)
                .Select(el => el.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return endTripLocationIds;
        }
    }
}
