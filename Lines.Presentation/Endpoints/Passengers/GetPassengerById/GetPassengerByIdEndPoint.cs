using Lines.Application.Features.Passengers.GetPassengerById.Orchestrators;
using Lines.Application.Features.Passengers.SharedDtos;
using Lines.Domain.Models.Passengers;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Passengers.GetById;

public class GetPassengerByIdEndPoint : BaseController<GetPassengerByIdRequest, GetPassengersDto>
{
    public GetPassengerByIdEndPoint(BaseControllerParams<GetPassengerByIdRequest> dependencyCollection)
        : base(dependencyCollection)
    {
    }

    [HttpGet("passengers/getbyid")]
    public async Task<ApiResponse<GetPassengersDto>> HandleAsync([FromQuery] GetPassengerByIdRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new GetPassengerByIdOrchestrator(request.Id), cancellationToken);

        return HandleResult<Passenger, GetPassengersDto>(result);
        //return ApiResponse<GetPassengersDto>.SuccessResponse(result, 200);
    }
}
