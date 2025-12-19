using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Dtos;
using Lines.Application.Features.VehicleTypes.GetallVehicleTypesWithExpectedPrice.Orchestrators;

namespace Lines.Presentation.Endpoints.VehicleTypes.GetAllVehicleTypesWithExpectedPrice
{
    public class GetAllVehicleTypesWithExpectedPriceEndPoint : BaseController<GetAllVehicleTypesWithExpectedPriceRequest, List<GetAllVehicleTypesWithExpectedPriceResponse>>

    {
        private readonly BaseControllerParams<GetAllVehicleTypesWithExpectedPriceRequest> _baseControllerParams;

        public GetAllVehicleTypesWithExpectedPriceEndPoint(BaseControllerParams<GetAllVehicleTypesWithExpectedPriceRequest> dependencyCollection) : base(dependencyCollection)
        {
            _baseControllerParams = dependencyCollection;
        }

        [HttpGet("vehicle-type-with-expected-price/getall")]
        public async Task<ApiResponse<List<GetAllVehicleTypesWithExpectedPriceResponse>>> Create(
          [FromQuery] GetAllVehicleTypesWithExpectedPriceRequest request,
          CancellationToken cancellationToken)
        {
            var validateRequest = await ValidateRequestAsync(request);
            if (!validateRequest.IsSuccess)
            {
                return validateRequest;
            }

            var result = await _mediator
                .Send(new GetallVehicleTypesWithExpectedPriceOrchestrator(request.Km , request.UserRewardID), cancellationToken)
                .ConfigureAwait(false);

            return HandleResult<List<GetAllVehicleTypesWithExpectedPriceDto>,
                                List<GetAllVehicleTypesWithExpectedPriceResponse>>(result);
        }

    }
}
