using Lines.Application.Features.Drivers.UpdateDriverContact.Orchestrators;
using Lines.Application.Features.Drivers.UpdateDriverContact.DTOs;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.UpdateDriverContact;

[Authorize]
public class UpdateDriverContactEndpoint : BaseController<UpdateDriverContactRequest, UpdateDriverContactResponse>
{
    public UpdateDriverContactEndpoint(BaseControllerParams<UpdateDriverContactRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [HttpPut("drivers/contact")]
    public async Task<ApiResponse<UpdateDriverContactResponse>> UpdateContact(UpdateDriverContactRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<UpdateDriverContactResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                return ApiResponse<UpdateDriverContactResponse>.ErrorResponse(
                    new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation), 401);
            }

            // Convert request to DTO
            var contactInfoDto = new UpdateDriverContactDto
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            // Call orchestrator
            var result = await _mediator.Send(new UpdateDriverContactOrchestrator(userId, contactInfoDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<UpdateDriverContactResponse>.ErrorResponse(result.Error, 400);
            }

            // Determine which fields were updated
            var updatedFields = new List<string>();
            if (!string.IsNullOrEmpty(request.Email))
                updatedFields.Add("Email");
            if (!string.IsNullOrEmpty(request.PhoneNumber))
                updatedFields.Add("PhoneNumber");

            var response = new UpdateDriverContactResponse
            {
                Success = true,
                Message = $"Contact information updated successfully. Updated: {string.Join(", ", updatedFields)}",
                EmailVerified = false, // Will be false if email was changed
                PhoneVerified = false, // Will be false if phone was changed
                UpdatedFields = string.Join(", ", updatedFields)
            };

            return ApiResponse<UpdateDriverContactResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<UpdateDriverContactResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("500", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 500);
        }
    }
}
