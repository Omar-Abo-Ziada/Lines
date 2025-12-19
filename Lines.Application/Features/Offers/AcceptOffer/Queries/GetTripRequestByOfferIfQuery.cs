using Lines.Application.Features.Offers.AcceptOffer.DTOs;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Offers.AcceptOffer.Queries;

public record GetTripRequestByOfferIfQuery(Guid offerId) : IRequest<GetOfferForAcceptDTO?>;

public class GetTripRequestByOfferIfQueryHandler(
    RequestHandlerBaseParameters parameters,
    IRepository<Offer> repository)
    : RequestHandlerBase<GetTripRequestByOfferIfQuery, GetOfferForAcceptDTO?>(parameters)
{
    private readonly IRepository<Offer> _offersRepository = repository;

    public override async Task<GetOfferForAcceptDTO?> Handle(GetTripRequestByOfferIfQuery request, CancellationToken cancellationToken)
    {
        var result = await _offersRepository
          .Get(o => o.Id == request.offerId)
          .ProjectToType<GetOfferForAcceptDTO>()
          .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}