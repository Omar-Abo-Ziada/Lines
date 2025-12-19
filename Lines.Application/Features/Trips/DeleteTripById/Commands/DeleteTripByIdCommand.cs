using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Trips.DeleteTripById.Commands
{
    public record DeleteTripByIdCommand(Guid TripId) : IRequest<bool>;


    public class DeleteTripByIdCommandHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<Trip> tripRepository)
        : RequestHandlerBase<DeleteTripByIdCommand, bool>(parameters)
    {
        public override async Task<bool> Handle(DeleteTripByIdCommand request, CancellationToken cancellationToken)
        {
         
            await tripRepository.DeleteAsync(request.TripId, cancellationToken);

            return true;
        }
    }
}
