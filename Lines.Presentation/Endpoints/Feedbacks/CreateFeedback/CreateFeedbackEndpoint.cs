using Lines.Application.Features.Feedbacks.CreateFeedback.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Feedbacks.CreateFeedback;

public class CreateFeedbackEndpoint : BaseController<CreateFeedbackRequest, CreateFeedbackResponse>
{
    private readonly BaseControllerParams<CreateFeedbackRequest> _dependencyCollection;

    public CreateFeedbackEndpoint(BaseControllerParams<CreateFeedbackRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }


    [HttpPost("Feedback/Create")]
    public async Task<ApiResponse<CreateFeedbackResponse>> HandleAsync([FromBody] CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var fromUserId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        var result = await _mediator.Send(new CreateFeedbackOrchestrator(request.TripId, fromUserId, userRole, request.Rating, request.Comment)
                                          , cancellationToken).ConfigureAwait(false);
        return HandleResult<Guid, CreateFeedbackResponse>(result);

    }
}
