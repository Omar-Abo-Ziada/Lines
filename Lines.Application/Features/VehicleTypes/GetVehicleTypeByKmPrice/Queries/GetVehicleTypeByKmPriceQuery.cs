using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Dtos;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Queries
{
    public record GetVehicleTypeByPriceQuery(Guid TripRequestId, decimal TotalPrice)
      : IRequest<GetVehicleTypeByPriceDto?>;

    public class GetVehicleTypeByPriceQueryHandler(
        RequestHandlerBaseParameters parameters,
        IRepository<VehicleType> vehicleTypeRepository,
        IRepository<TripRequest> tripRequestRepository)
        : RequestHandlerBase<GetVehicleTypeByPriceQuery, GetVehicleTypeByPriceDto?>(parameters)
    {
        private readonly IRepository<VehicleType> _vehicleTypeRepository = vehicleTypeRepository;
        private readonly IRepository<TripRequest> _tripRequestRepository = tripRequestRepository;

        public override async Task<GetVehicleTypeByPriceDto?> Handle(
            GetVehicleTypeByPriceQuery request,
            CancellationToken cancellationToken)
        {
            // 🧩 أولاً نجيب الـ TripRequest عشان ناخد المسافة (Distance)
            var tripRequest = await _tripRequestRepository
                .Get(tr => tr.Id == request.TripRequestId && !tr.IsDeleted)
                .Select(tr => new { tr.Distance })
                .FirstOrDefaultAsync(cancellationToken);

            if (tripRequest is null)
                throw new InvalidOperationException("Trip request not found.");

            if (tripRequest.Distance <= 0)
                throw new InvalidOperationException("Trip distance must be greater than zero.");

            // 🧮 نحسب السعر لكل كم بناءً على Distance في الطلب
            var pricePerKm = request.TotalPrice / (decimal)tripRequest.Distance;

            // 🚗 نختار أول نوع مركبة يكون سعرها <= السعر المحسوب لكل كم
            var vehicleType = await _vehicleTypeRepository
                .Get(v => !v.IsDeleted && v.PerKmCharge <= pricePerKm)
                .OrderByDescending(v => v.PerKmCharge)
                .Select(v => new GetVehicleTypeByPriceDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Capacity = v.Capacity,
                    PerKmCharge = v.PerKmCharge,
                    PerMinuteDelayCharge = v.PerMinuteDelayCharge
                })
                .FirstOrDefaultAsync(cancellationToken);

            return vehicleType;
        }
    }
 


}
