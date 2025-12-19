using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Dtos;
using Lines.Application.Features.VehicleTypes.GetVehicleTypeByKmPrice.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes
{
    public class GetVehicleTypeByKmPriceEndpoint
        : BaseController<GetVehicleTypeByKmPriceRequest, GetVehicleTypeByKmPriceResponse>
    {
        private readonly BaseControllerParams<GetVehicleTypeByKmPriceRequest> _baseControllerParams;

        public GetVehicleTypeByKmPriceEndpoint(BaseControllerParams<GetVehicleTypeByKmPriceRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _baseControllerParams = dependencyCollection;
        }

        /// <summary>
        /// Retrieves the most suitable vehicle type based on the provided trip information.
        /// </summary>
        /// <param name="request">
        /// The trip details containing the total trip price and total kilometers to calculate the price per kilometer.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing the matched <see cref="GetVehicleTypeByKmPriceResponse"/> 
        /// if a suitable vehicle type is found; otherwise, an error response describing the failure reason.
        /// </returns>
        /// <remarks>
        /// This endpoint calculates the price per kilometer from the total trip price and distance,
        /// then returns the closest vehicle type whose per-kilometer charge is less than or equal to the calculated value.
        /// </remarks>
        [HttpGet("vehicle-types/suggest")]
        public async Task<ApiResponse<GetVehicleTypeByKmPriceResponse>> GetByTrip(
            [FromQuery] GetVehicleTypeByKmPriceRequest request,
            CancellationToken cancellationToken)
        {
            var validateRequest = await ValidateRequestAsync(request);
            if (!validateRequest.IsSuccess)
                return validateRequest;

            var result = await _mediator.Send(
                new GetVehicleTypeByKmPriceOrchestrator(request.TripRequestId, request.TotalPrice),
                cancellationToken).ConfigureAwait(false);

            return HandleResult<GetVehicleTypeByPriceDto, GetVehicleTypeByKmPriceResponse>(result);
        }
    }
}
