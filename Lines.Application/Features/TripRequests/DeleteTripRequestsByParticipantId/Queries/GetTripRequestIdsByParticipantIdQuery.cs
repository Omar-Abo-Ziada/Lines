using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;
using System.Data.Entity;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestsByParticipantId.Queries
{
    public record GetTripRequestIdsByParticipantIdQuery(Guid ParticipantId) : IRequest<List<Guid>>;


    public class GetTripRequestIdsByParticipantIdQueryHandler(
               RequestHandlerBaseParameters parameters, IRepository<TripRequest> tripRequestRepository)
        : RequestHandlerBase<GetTripRequestIdsByParticipantIdQuery, List<Guid>>(parameters)
    {
        public override async Task<List<Guid>> Handle(GetTripRequestIdsByParticipantIdQuery request, CancellationToken cancellationToken)
        {
            var tripRequestIds = await tripRequestRepository.Get(tr => tr.DriverId == request.ParticipantId ||
                                                                 tr.PassengerId == request.ParticipantId)
                                                             .Select(tr => tr.Id)
                                                             .ToListAsync()
                                                             .ConfigureAwait(false);                   

            return tripRequestIds;
        }
    }
}
