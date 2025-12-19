using Lines.Application.Features.Activities.CreateNewActivity.Orchestrators;

namespace Lines.Presentation.Endpoints.Activities.CreateNewActivity
{
    public class CreateNewActivityEndpoint: BaseController<CreateNewActivityRequest, bool>
    {
        private readonly BaseControllerParams<CreateNewActivityRequest> _dependencyCollection;

        public CreateNewActivityEndpoint(
            BaseControllerParams<CreateNewActivityRequest> dependencyCollection
        ) : base(dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        [HttpPost("Activities/Create")]
        public async Task<ApiResponse<bool>> HandleAsync(
            CreateNewActivityRequest request,
            CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var passengerId = GetCurrentPassengerId();
            if (userId == Guid.Empty)
            {
                var error = new Error("Authentication.Error", "User is not authenticated.", ErrorType.NotAuthenticated);
                return ApiResponse<bool>.ErrorResponse(error, 401);
            }

            // ✅ Send orchestrator
            var result = await _mediator.Send(new CreateNewActivityOrchestrator(userId, passengerId),cancellationToken);

            // ✅ Return API response
            return result.IsSuccess
                ? ApiResponse<bool>.SuccessResponse(true, 200)
                : ApiResponse<bool>.ErrorResponse(result.Error, 400);
        }
    }
}
