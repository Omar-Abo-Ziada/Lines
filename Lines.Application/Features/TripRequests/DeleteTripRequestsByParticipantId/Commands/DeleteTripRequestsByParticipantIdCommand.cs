using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestsByUserId.Commands
{
    public record DeleteTripRequestsByParticipantIdCommand(Guid ParticipantId) : IRequest<bool>;

    public class DeleteTripRequestsByParticipantIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<TripRequest> tripRequestRepository)
         : RequestHandlerBase<DeleteTripRequestsByParticipantIdCommand, bool>(parameters)
    {
        private readonly IRepository<TripRequest> _tripRequestRepository = tripRequestRepository;

        public override async Task<bool> Handle(DeleteTripRequestsByParticipantIdCommand request, CancellationToken cancellationToken)
        {
            var tripRequests = _tripRequestRepository.Get(x => x.PassengerId == request.ParticipantId 
                                                       || x.DriverId == request.ParticipantId)
                                                     .ToList();
            
            foreach (var tripRequest in tripRequests)
            {
                await _tripRequestRepository.DeleteAsync(tripRequest.Id);
            }

            return true;
        }
    }
} 