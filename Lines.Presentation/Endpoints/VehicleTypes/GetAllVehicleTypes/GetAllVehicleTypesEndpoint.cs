using Lines.Application.Features.VehicleTypes.DTOs;
using Lines.Application.Features.VehicleTypes.Orchestrators;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public class GetAllVehicleTypesEndpoint : BaseController<GetAllVehicleTypesRequest,  PagingDto<GetAllVehicleTypesResponse>>
{
    private readonly BaseControllerParams<GetAllVehicleTypesRequest>  _baseControllerParams;
    public GetAllVehicleTypesEndpoint(BaseControllerParams<GetAllVehicleTypesRequest> dependencyCollection) : base(dependencyCollection)
    {
        _baseControllerParams = dependencyCollection;
    }

    [HttpGet("vehicle-type/getall")]
    public async Task<ApiResponse<PagingDto<GetAllVehicleTypesResponse>>> Create([FromQuery] GetAllVehicleTypesRequest request, CancellationToken cancellationToken)
    {
        var validateRequest = await ValidateRequestAsync(request);
        if (!validateRequest.IsSuccess)
        {
            return validateRequest;
        }
        var result = await _mediator.Send(new GetAllVehicleTypesOrchestrator(request.Name, request.Capacity, request.PageSize ,request.PageNumber), cancellationToken).ConfigureAwait(false);
        return HandleResult<PagingDto<GetAllVehicleTypesDto>, PagingDto<GetAllVehicleTypesResponse>>(result);
    }
}