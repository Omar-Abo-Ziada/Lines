using Lines.Application.Features.Offers.GetOfferByRequestId.DTOs;
using Lines.Application.Helpers;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.Offers.GetOfferByRequestId.Queries
{
    public record GetOfferByRequestIdQuery(Guid RequestId, decimal? DiscountPercentage, decimal? MaxValue) : IRequest<GetOfferByRequestIdResult>;
    public class GetOfferByRequestIdHandler(
      RequestHandlerBaseParameters parameters,
      IRepository<Offer> offersRepository)
      : RequestHandlerBase<GetOfferByRequestIdQuery, GetOfferByRequestIdResult>(parameters)
    {
        public override async Task<GetOfferByRequestIdResult> Handle(GetOfferByRequestIdQuery request, CancellationToken cancellationToken)
        {
            bool isRewardApplied = request.DiscountPercentage.HasValue && request.MaxValue.HasValue;  // consider reward must always has 2 values max and discount

            var offers = await offersRepository
                .Get(n => n.TripRequestId == request.RequestId)
                .ToListAsync();

            var data = offers.Select(o => new GetOfferByRequestIdDTO
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                DistanceToArriveInMeters = o.DistanceToArriveInMeters,
                Price = o.Price,
                TripRequestId = o.TripRequestId,
                DriverId = o.DriverId,
                IsAccepted = o.IsAccepted,
                TimeToArriveInMinutes = o.TimeToArriveInMinutes,
                PriceAfterRewardApplied = isRewardApplied ? FareAfterRewardAppliedCalculator.Calculate(o.Price , (decimal)request.DiscountPercentage, (decimal)request.MaxValue)
                                                          : null
            }).ToList();

            var result = new GetOfferByRequestIdResult
            {
                Data = data,
                Total = data.Count
            };
            return result;
        }
    }
}
