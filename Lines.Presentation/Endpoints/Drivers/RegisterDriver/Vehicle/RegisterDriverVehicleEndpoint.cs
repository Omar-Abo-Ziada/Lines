using Lines.Application.Features.Drivers.RegisterDriver.DTOs;
using Lines.Application.Features.Drivers.RegisterDriver.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.RegisterDriver.Vehicle;

public class RegisterDriverVehicleEndpoint : BaseController<RegisterDriverVehicleRequest, RegisterDriverVehicleResponse>
{
    public RegisterDriverVehicleEndpoint(BaseControllerParams<RegisterDriverVehicleRequest> dependencyCollection) : base(dependencyCollection)
    {
    }

    [HttpPost("drivers/register/step5/vehicle")]
    public async Task<ApiResponse<RegisterDriverVehicleResponse>> HandleAsync(
        RegisterDriverVehicleRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate request first
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
            {
                return ApiResponse<RegisterDriverVehicleResponse>.ErrorResponse(validationResult.Error!, validationResult.StatusCode);
            }

            // Convert request to DTO
            var vehicleDto = new RegisterDriverVehicleDto(
                request.VehicleTypeId,
                request.Model,
                request.Year,
                request.Color,
                request.LicensePlate,
                request.RegistrationPapers,
                request.KmPrice
            );

            // Call orchestrator
            var result = await _mediator.Send(new RegisterDriverVehicleOrchestrator(request.RegistrationToken, vehicleDto), cancellationToken);

            if (result.IsFailure)
            {
                return ApiResponse<RegisterDriverVehicleResponse>.ErrorResponse(result.Error, 400);
            }

            var response = new RegisterDriverVehicleResponse
            {
                Success = true,
                Message = "Vehicle information saved successfully. Please proceed to withdrawal information.",
                CurrentStep = 5
            };

            return ApiResponse<RegisterDriverVehicleResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterDriverVehicleResponse>.ErrorResponse(
                new Lines.Application.Shared.Error("General.Error", $"An error occurred: {ex.Message}", Lines.Application.Shared.ErrorType.Failure), 
                500);
        }
    }
}
