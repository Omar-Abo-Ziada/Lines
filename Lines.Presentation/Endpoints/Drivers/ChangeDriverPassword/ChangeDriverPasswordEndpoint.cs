using Lines.Application.Features.Users.UpdatePassword.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.ChangeDriverPassword;

[Authorize]
public class ChangeDriverPasswordEndpoint : BaseController<ChangeDriverPasswordRequest, ChangeDriverPasswordResponse>
{
    public ChangeDriverPasswordEndpoint(BaseControllerParams<ChangeDriverPasswordRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpPut("drivers/change-password")]
    public async Task<ApiResponse<ChangeDriverPasswordResponse>> ChangePassword(ChangeDriverPasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<ChangeDriverPasswordResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<ChangeDriverPasswordResponse>.ErrorResponse(
                    new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
            }

            var result = await _mediator.Send(new UpdatePasswordOrchestrator(userId, request.NewPassword, request.CurrentPassword), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<ChangeDriverPasswordResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new ChangeDriverPasswordResponse
            {
                Success = true,
                Message = "Password changed successfully."
            };

            return ApiResponse<ChangeDriverPasswordResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<ChangeDriverPasswordResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("500", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 500);
        }
    }
}
