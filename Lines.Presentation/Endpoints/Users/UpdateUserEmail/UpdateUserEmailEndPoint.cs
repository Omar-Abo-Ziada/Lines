using Lines.Application.Features.Drivers.UpdateDriverContact.DTOs;
using Lines.Application.Features.Drivers.UpdateDriverContact.Orchestrators;
using Lines.Application.Features.Users.UpdateUserEmail.DTOs;
using Lines.Application.Features.Users.UpdateUserEmail.Orchestrators;
using Lines.Presentation.Endpoints.Drivers.UpdateDriverContact;
using Lines.Presentation.Endpoints.Passengers;

namespace Lines.Presentation.Endpoints.Users.UpdateUserEmail
{

    public class UpdateUserEmailEndPoint : BaseController<UpdateUserEmailRequest, UpdateUserEmailResponse>
    {
        public UpdateUserEmailEndPoint(BaseControllerParams<UpdateUserEmailRequest> dependencyCollection): base(dependencyCollection)
        {
        }
        [HttpPost("users/updateUserEmail")]
        public async Task<ApiResponse<UpdateUserEmailResponse>> HandleAsync(UpdateUserEmailRequest request, CancellationToken cancellationToken)
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                //return ApiResponse<UpdateUserEmailResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
                return validationResult;
            }


            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<UpdateUserEmailResponse>.ErrorResponse(
                    new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
            }

            // Convert request to DTO
            var dto = new UpdateUserEmailDto
            {
                CurrentEmail = request.CurrentEmail,
                NewEmail = request.NewEmail,
            };


            // Call orchestrator
            var result = await _mediator.Send(new UpdateUserEmailOrchestrator(userId, dto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<UpdateUserEmailResponse>.ErrorResponse(result.Error, 400);
            }

            // Determine which fields were updated
            var updatedFields = new List<string>();
            if (!string.IsNullOrEmpty(request.NewEmail))
                updatedFields.Add("NewEmail");
            //if (!string.IsNullOrEmpty(request.PhoneNumber))
            //    updatedFields.Add("PhoneNumber");

            var response = new UpdateUserEmailResponse
            {
                Success = true,
                Message = $"Email updated successfully. Updated: {string.Join(", ", updatedFields)}",
                EmailVerified = false, // Will be false if email was changed
                UpdatedFields = string.Join(", ", updatedFields)
            };

            return ApiResponse<UpdateUserEmailResponse>.SuccessResponse(response);
        }
    }
}
