
using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Dtos;
using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Queries;

namespace Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Orchestrators
{
    public record GetVehicleTypeByKmPriceOrchestrator(Guid TripRequestId, decimal TotalPrice)
        : IRequest<Result<GetVehicleTypeByPriceDto?>>;

    public class GetVehicleTypeByKmPriceOrchestratorHandler(
        RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetVehicleTypeByKmPriceOrchestrator, Result<GetVehicleTypeByPriceDto?>>(parameters)
    {
        public override async Task<Result<GetVehicleTypeByPriceDto?>> Handle(
            GetVehicleTypeByKmPriceOrchestrator request,
            CancellationToken cancellationToken)
        {
            // ✅ تحقق من السعر
            if (request.TotalPrice <= 0)
            {
                return Result<GetVehicleTypeByPriceDto?>.Failure(
                    new Error("Invalid.Input", "TotalPrice must be greater than zero.", ErrorType.Validation));
            }

             var vehicleType = await _mediator.Send(
                new GetVehicleTypeByPriceQuery(request.TripRequestId, request.TotalPrice),
                cancellationToken);

             if (vehicleType is null)
                return Result<GetVehicleTypeByPriceDto?>.Success(null);

             return Result<GetVehicleTypeByPriceDto?>.Success(vehicleType);
        }
    }
}

 
