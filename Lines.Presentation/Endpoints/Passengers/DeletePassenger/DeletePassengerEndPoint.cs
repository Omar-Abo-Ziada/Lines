using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.Passengers.DeletePassenger;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Passengers
{
    public class DeletePassengerEndpoint : BaseController<DeletePassengerRequest, bool>
    {
        private readonly BaseControllerParams<DeletePassengerRequest> _dependencyCollection;

        public DeletePassengerEndpoint(BaseControllerParams<DeletePassengerRequest> dependencyCollection)
            : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpDelete("passenger/delete/{Id}")]
        public async Task<ApiResponse<bool>> Delete([FromRoute] DeletePassengerRequest request)
        {
            var validateResult = await ValidateRequestAsync(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            var result = await _mediator.Send(new DeletePassengerOrchestrator(request.Id));
            return HandleResult<bool, bool>(result);
        }
    }
}
