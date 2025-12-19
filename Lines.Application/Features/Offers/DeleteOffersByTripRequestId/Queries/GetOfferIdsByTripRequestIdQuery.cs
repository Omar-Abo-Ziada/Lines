using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using MediatR;
using System.Data.Entity;

namespace Lines.Application.Features.Offers.DeleteOffersByTripRequestId.Queries
{
    public record GetOfferIdsByTripRequestIdQuery(Guid TripRequestId) : IRequest<List<Guid>>;



    public class GetOfferIdsByTripRequestIdQueryHandler : IRequestHandler<GetOfferIdsByTripRequestIdQuery, List<Guid>>
    {
        private readonly IRepository<Offer> _offerRepository;

        public GetOfferIdsByTripRequestIdQueryHandler(IRepository<Offer> offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<Guid>> Handle(GetOfferIdsByTripRequestIdQuery request, CancellationToken cancellationToken)
        {
            return await _offerRepository.Get(o => o.TripRequestId == request.TripRequestId)
                                         .Select(o => o.Id)
                                         .ToListAsync();
        }
    }
}
