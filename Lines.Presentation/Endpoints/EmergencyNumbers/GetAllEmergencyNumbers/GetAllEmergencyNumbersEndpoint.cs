using Lines.Application.Extensions;
using Lines.Application.Features.EmergencyNumbers;
using Lines.Application.Features.EmergencyNumbers.Shared.DTOs;

namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public class GetAllEmergencyNumbersEndpoint : BaseController<GetAllEmergencyNumbersRequest, PagingDto<GetAllEmergencyNumbersResponse>>
{
    private readonly BaseControllerParams<GetAllEmergencyNumbersRequest> _dependencyCollection;

    public GetAllEmergencyNumbersEndpoint(BaseControllerParams<GetAllEmergencyNumbersRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }
    [HttpGet("emergencynumber/getall")]
    public async Task<ApiResponse<PagingDto<GetAllEmergencyNumbersResponse>>> HandleAsync([FromQuery] GetAllEmergencyNumbersRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new GetAllEmergencyNumbersQuery(request.Name, request.PhoneNumber, request.EmergencyNumberType, request.PageNumber, request.PageSize, userId),
                                          cancellationToken).ConfigureAwait(false);
            
        return ApiResponse<PagingDto<GetAllEmergencyNumbersResponse>>
            .SuccessResponse(result.AdaptPaging<GetEmergencyNumberDto, GetAllEmergencyNumbersResponse>(), 200);
    }
}