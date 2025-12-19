using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Interfaces;
using Lines.Domain.Models.Drivers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.RewardOffers.GetActiveOffer.Queries;

public record GetActiveOfferQuery(Guid DriverId) : IRequest<Result<ActiveOfferDto>>;

public class GetActiveOfferQueryHandler : RequestHandlerBase<GetActiveOfferQuery, Result<ActiveOfferDto>>
{
    private readonly IRepository<DriverOfferActivation> _activationRepository;

    public GetActiveOfferQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<DriverOfferActivation> activationRepository)
        : base(parameters)
    {
        _activationRepository = activationRepository;
    }

    public override async Task<Result<ActiveOfferDto>> Handle(
        GetActiveOfferQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var activation = await _activationRepository
            .Get(a => a.DriverId == request.DriverId && a.IsActive && a.ExpirationDate > now)
            .Include(a => a.Offer)
            .OrderByDescending(a => a.ActivationDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (activation == null)
        {
            return Result<ActiveOfferDto>.Success(null);
        }

        var dto = new ActiveOfferDto
        {
            ActivationId = activation.Id,
            OfferId = activation.OfferId,
            Title = activation.Offer.Title,
            Description = activation.Offer.Description,
            ServiceFeePercent = activation.Offer.ServiceFeePercent,
            ActivationDate = activation.ActivationDate,
            ExpirationDate = activation.ExpirationDate,
            RemainingDays = activation.RemainingDays(),
            IsActive = activation.IsActive
        };

        return Result<ActiveOfferDto>.Success(dto);
    }
}

