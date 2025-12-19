using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestById.Commands
{
    public record DeleteTripRequestByIdCommand(Guid TripRequestId) : IRequest<bool>;


    public class DeleteTripRequestByIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<TripRequest> _tripRequestRepository)
         : RequestHandlerBase<DeleteTripRequestByIdCommand, bool>(parameters) 
    {

        public override async Task<bool> Handle(DeleteTripRequestByIdCommand request, CancellationToken cancellationToken)
        {
            await _tripRequestRepository.DeleteAsync(request.TripRequestId, cancellationToken);
            return true; // Deletion successful
        }
    }
}
