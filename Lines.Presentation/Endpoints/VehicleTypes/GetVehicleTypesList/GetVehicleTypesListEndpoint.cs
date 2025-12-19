using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class GetVehicleTypesListEndpoint : BaseController<GetVehicleTypesListRequest,  List<GetVehicleTypesListResponse>>
{
    private readonly BaseControllerParams<GetVehicleTypesListRequest>  _baseControllerParams;
    public GetVehicleTypesListEndpoint(BaseControllerParams<GetVehicleTypesListRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpGet("vehicle-type/getlist")]
    public async Task<ApiResponse<List<GetVehicleTypesListResponse>>> Create([FromQuery] GetVehicleTypesListRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new GetVehicleTypesListOrchestrator(request.Latitude, request.Longitude), cancellationToken).ConfigureAwait(false);
        return HandleResult<List<GetVehicleTypesListDto>, List<GetVehicleTypesListResponse>>(result);
    }
}