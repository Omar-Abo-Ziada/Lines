using Lines.Application.Features.Otps.ValidateOtpByUserId.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Otps.ValidateOtp
{
    public class ValidateOtpEndpoint : BaseController<ValidateOtpRequest, bool>  
    {
        private readonly BaseControllerParams<ValidateOtpRequest> _dependencyCollection;

        public ValidateOtpEndpoint(BaseControllerParams<ValidateOtpRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("otps/validate")]  
        public async Task<ApiResponse<bool>> HandleAsync(ValidateOtpRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            Domain.Shared.Result<Application.Features.Otps.ValidateOtpByUserId.DTOs.ValidateOtpByUserIdDto> result = await _mediator.Send(
                            new ValidateOtpByUserIdOrchestrator(request.UserId, request.Otp),
                            cancellationToken);

            return result.IsSuccess
                ? HandleResult<bool, bool>(result.Value.IsValid)
                : ApiResponse<bool>.ErrorResponse(result.Error, 400);
        }
    }
}
