using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;
using System.Data.Entity;

namespace Lines.Application.Features.Trips.DeleteTripsByParticipantId.Queries
{

    public record GetTripIdsByParticipantIdQuery(Guid ParticipantId) : IRequest<List<Guid>>;

    public class GetTripIdsByParticipantIdHandler(
      RequestHandlerBaseParameters parameters,
      IRepository<Trip> tripRepository)
      : RequestHandlerBase<GetTripIdsByParticipantIdQuery, List<Guid>>(parameters)
    {
        public override async Task<List<Guid>> Handle(GetTripIdsByParticipantIdQuery request, CancellationToken cancellationToken)
        {

            var tripIds = await tripRepository
                .Get(t => t.PassengerId == request.ParticipantId || t.DriverId == request.ParticipantId)
                .Select(t => t.Id).ToListAsync();

            return tripIds;
        }
    }
}
