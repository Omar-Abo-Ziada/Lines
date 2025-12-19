using Lines.Application.Extensions;
using Lines.Application.Features.Passengers.GetAllPassengers.Queries;
using Lines.Application.Features.Passengers.SharedDtos;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Passengers.GetAllPassengers
{
    public class GetAllPassengersEndPoint : BaseController <GetAllPassengersRequest , PagingDto<GetAllPassengersResponse>>
    {
        public GetAllPassengersEndPoint(BaseControllerParams<GetAllPassengersRequest> dependencyCollection) : base(dependencyCollection)
        {
            
        }
        


        [HttpGet("passengers/getall")]
        public async Task<ApiResponse<PagingDto<GetAllPassengersResponse>>> HandleAsync([FromQuery] GetAllPassengersRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
            var result = await _mediator.Send(new GetAllPassengersQuery( request.FirstName , request.LastName , request.Email , request.PhoneNumber ,
                request.PageNumber, request.PageSize), cancellationToken).ConfigureAwait(false);

            return ApiResponse<PagingDto<GetAllPassengersResponse>>
                .SuccessResponse(result.AdaptPaging<GetPassengersDto, GetAllPassengersResponse>(), 200);
        }
    }
}
