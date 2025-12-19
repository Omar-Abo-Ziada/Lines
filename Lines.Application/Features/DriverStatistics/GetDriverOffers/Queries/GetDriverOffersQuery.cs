using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.DriverStatistics.GetDriverOffers.Queries;

public record GetDriverOffersQuery(Guid DriverId) : IRequest<Result<List<DriverOfferDto>>>;

public class GetDriverOffersQueryHandler : RequestHandlerBase<GetDriverOffersQuery, Result<List<DriverOfferDto>>>
{
    private readonly IRepository<DriverServiceFeeOffer> _offerRepository;

    public GetDriverOffersQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverServiceFeeOffer> offerRepository) : base(parameters)
    {
        _offerRepository = offerRepository;
    }

    public override async Task<Result<List<DriverOfferDto>>> Handle(GetDriverOffersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var now = DateTime.UtcNow;

            // Get active and upcoming offers (not expired)
            var offers = await _offerRepository
                .Get(o => o.DriverId == request.DriverId && o.ValidUntil >= now)
                .OrderBy(o => o.ValidFrom)
                .ToListAsync(cancellationToken);

            var result = offers.Select(o => new DriverOfferDto
            {
                ServiceFee = Math.Round(o.ServiceFeePercent, 2),
                ValidFrom = o.IsActive() ? null : o.ValidFrom,
                ValidUntil = o.ValidUntil
            }).ToList();

            return Result<List<DriverOfferDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<DriverOfferDto>>.Failure(Error.General);
        }
    }
}


