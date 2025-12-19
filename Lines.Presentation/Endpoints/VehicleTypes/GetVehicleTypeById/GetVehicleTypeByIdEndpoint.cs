using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class GetVehicleTypeByIdEndpoint : BaseController<GetVehicleTypeByIdRequest,  GetVehicleTypeByIdResponse>
{
    private readonly BaseControllerParams<GetVehicleTypeByIdRequest>  _baseControllerParams;
    public GetVehicleTypeByIdEndpoint(BaseControllerParams<GetVehicleTypeByIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpGet("vehicle-type/getbyid")]
    public async Task<ApiResponse<GetVehicleTypeByIdResponse>> Create([FromQuery] GetVehicleTypeByIdRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new GetVehicleTypeByIdOrchestrator(request.Id), cancellationToken).ConfigureAwait(false);
        return HandleResult<GetVehicleTypeByIdDto, GetVehicleTypeByIdResponse>(result);
    }
}