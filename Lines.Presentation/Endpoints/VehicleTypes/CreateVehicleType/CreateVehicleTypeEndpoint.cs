using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class CreateVehicleTypeEndpoint : BaseController<CreateVehicleTypeRequest,  CreateVehicleTypeResponse>
{
    private readonly BaseControllerParams<CreateVehicleTypeRequest>  _baseControllerParams;
    public CreateVehicleTypeEndpoint(BaseControllerParams<CreateVehicleTypeRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpPost("vehicle-type/create")]
    public async Task<ApiResponse<CreateVehicleTypeResponse>> Create(CreateVehicleTypeRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new CreateVehicleTypeOrchestrator(request.Name, request.Description, request.Capacity, request.PerKmCharge, request.PerMinuteDelayCharge, request.AverageSpeedKmPerHour), cancellationToken).ConfigureAwait(false);
        return HandleResult<CreateVehicleTypeDto, CreateVehicleTypeResponse>(result);
    }
}