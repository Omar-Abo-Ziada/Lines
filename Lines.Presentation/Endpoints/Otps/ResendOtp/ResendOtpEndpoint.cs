using Lines.Application.Features.Otps.ResendOtp.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Otps.ResendOtp
{
    public class ResendOtpEndpoint : BaseController<ResendOtpRequest , Guid>
    {
        private BaseControllerParams<ResendOtpRequest> _dependencyCollection;
        public ResendOtpEndpoint(BaseControllerParams<ResendOtpRequest> dependencyCollection) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }


        [HttpPost("otps/resend")]  
        public async Task<ApiResponse<Guid>> HandleAsync(ResendOtpRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var result = await _mediator.Send(new ResendOtpOrchestrator(request.UserId , request.Email), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<Guid>.ErrorResponse(result.Error, 400);
            }

            return result.IsSuccess ?
                ApiResponse<Guid>.SuccessResponse(result.Value, 200) :
                ApiResponse<Guid>.ErrorResponse(result.Error, 400);
        }
    }
    

}
