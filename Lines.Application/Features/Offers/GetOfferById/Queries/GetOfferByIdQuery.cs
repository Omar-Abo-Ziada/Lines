using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Offers.GetOfferById.Queries
{
    public record GetOfferByIdQuery(Guid Id) : IRequest<Offer>;


    public class GetOfferByIdQueryHandler(RequestHandlerBaseParameters parameters, IRepository<Offer> _repository)
        : RequestHandlerBase<GetOfferByIdQuery, Offer>(parameters)
    {
        public override async Task<Offer> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _repository.Get(o => o.Id == request.Id)
                                         .FirstOrDefaultAsync();  
            return offer!;
        }
    }
}
