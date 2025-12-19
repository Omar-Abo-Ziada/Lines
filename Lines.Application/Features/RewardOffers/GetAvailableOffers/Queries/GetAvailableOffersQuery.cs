using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.RewardOffers.GetAvailableOffers.Queries;

public record GetAvailableOffersQuery() : IRequest<Result<List<AvailableOfferDto>>>;

public class GetAvailableOffersQueryHandler : RequestHandlerBase<GetAvailableOffersQuery, Result<List<AvailableOfferDto>>>
{
    private readonly IRepository<DriverServiceFeeOffer> _offerRepository;

    public GetAvailableOffersQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverServiceFeeOffer> offerRepository)
        : base(parameters)
    {
        _offerRepository = offerRepository;
    }

    public override async Task<Result<List<AvailableOfferDto>>> Handle(
        GetAvailableOffersQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var offers = await _offerRepository
            .Get(o => o.IsGloballyActive && o.ValidFrom <= now && o.ValidUntil >= now)
            .OrderBy(o => o.Price)
            .ToListAsync(cancellationToken);

        var offerDtos = offers.Adapt<List<AvailableOfferDto>>();

        return Result<List<AvailableOfferDto>>.Success(offerDtos);
    }
}

