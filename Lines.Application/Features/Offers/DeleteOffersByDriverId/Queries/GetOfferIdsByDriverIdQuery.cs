using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using MediatR;
using System.Data.Entity;

namespace Lines.Application.Features.Offers.DeleteOffersByDriverId.Queries
{
    public record GetOfferIdsByDriverIdQuery(Guid DriverId) : IRequest<List<Guid>>;
    
    public class GetOfferIdsByDriverIdQueryHandler : IRequestHandler<GetOfferIdsByDriverIdQuery, List<Guid>>
    {
        private readonly IRepository<Offer> _offerRepository;

        public GetOfferIdsByDriverIdQueryHandler(IRepository<Offer> offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<Guid>> Handle(GetOfferIdsByDriverIdQuery request, CancellationToken cancellationToken)
        {
            return await _offerRepository.Get(o => o.DriverId == request.DriverId)
                                         .Select(o => o.Id)
                                         .ToListAsync();
        }
    }


}
