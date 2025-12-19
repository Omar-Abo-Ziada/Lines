using Lines.Application.Common;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Trips;
using MediatR;

namespace Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByUserId.Commands
{
    public record DeleteEndTripLocationsByIdCommand(Guid Id) : IRequest<bool>;

    public class DeleteEndTripLocationsByIdCommandHandler(
         RequestHandlerBaseParameters parameters,
         IRepository<EndTripLocation> endTripLocationRepository)
         : RequestHandlerBase<DeleteEndTripLocationsByIdCommand, bool>(parameters)
    {
        private readonly IRepository<EndTripLocation> _endTripLocationRepository = endTripLocationRepository;

        public override async Task<bool> Handle(DeleteEndTripLocationsByIdCommand request, CancellationToken cancellationToken)
        {
                await _endTripLocationRepository.DeleteAsync(request.Id, cancellationToken);
                return true;
        }
    }
} 