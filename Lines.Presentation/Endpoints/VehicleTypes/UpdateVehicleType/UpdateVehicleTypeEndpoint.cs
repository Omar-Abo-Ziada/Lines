using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class UpdateVehicleTypeEndpoint : BaseController<UpdateVehicleTypeRequest,  bool>
{
    private readonly BaseControllerParams<UpdateVehicleTypeRequest>  _baseControllerParams;
    public UpdateVehicleTypeEndpoint(BaseControllerParams<UpdateVehicleTypeRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpPut("vehicle-type/update")]
    public async Task<ApiResponse<bool>> Create(UpdateVehicleTypeRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new UpdateVehicleTypeOrchestrator(request.Id, request.Name, request.Description, request.Capacity, request.PerKmCharge, request.PerMinuteDelayCharge), cancellationToken).ConfigureAwait(false);
        return HandleResult<bool, bool>(result);
    }
}