using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Withdrawal;

public class RegisterDriverWithdrawalEndpoint : BaseController<RegisterDriverWithdrawalRequest, RegisterDriverWithdrawalResponse>
{
    public RegisterDriverWithdrawalEndpoint(BaseControllerParams<RegisterDriverWithdrawalRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step6/withdrawal")]
    public async Task<ApiResponse<RegisterDriverWithdrawalResponse>> HandleAsync(
        RegisterDriverWithdrawalRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverWithdrawalResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var withdrawalDto = new RegisterDriverWithdrawalDto(
                request.BankName,
                request.IBAN,
                request.SWIFT,
                request.AccountHolderName
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverWithdrawalOrchestrator(request.RegistrationToken, withdrawalDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverWithdrawalResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverWithdrawalResponse
            {
                Success = true,
                Message = "Driver registration completed successfully! Your account is now active.",
                DriverId = result.Value,
                UserId = result.Value, // Assuming DriverId and UserId are the same
                CurrentStep = 6,
                RegistrationComplete = true
            };

            return ApiResponse<RegisterDriverWithdrawalResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverWithdrawalResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
