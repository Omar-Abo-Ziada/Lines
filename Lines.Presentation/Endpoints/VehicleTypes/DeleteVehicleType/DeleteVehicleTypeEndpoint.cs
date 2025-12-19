using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class DeleteVehicleTypeEndpoint : BaseController<DeleteVehicleTypeRequest,  bool>
{
    private readonly BaseControllerParams<DeleteVehicleTypeRequest>  _baseControllerParams;
    public DeleteVehicleTypeEndpoint(BaseControllerParams<DeleteVehicleTypeRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpDelete("vehicle-type/delete")]
    public async Task<ApiResponse<bool>> Create(DeleteVehicleTypeRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new DeleteVehicleTypeOrchestrator(request.Id), cancellationToken).ConfigureAwait(false);
        return HandleResult<bool, bool>(result);
    }
}