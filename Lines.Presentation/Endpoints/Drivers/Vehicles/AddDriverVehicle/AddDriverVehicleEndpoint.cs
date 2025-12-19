using Lines.Application.Features.Vehicles.AddDriverVehicle.DTOs;
using Lines.Application.Features.Vehicles.AddDriverVehicle.Orchestrators;
using Lines.Application.Features.Common.Commands;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.AddDriverVehicle;

public class AddDriverVehicleEndpoint : BaseController<AddDriverVehicleRequest, AddDriverVehicleResponse>
{
    private readonly BaseControllerParams<AddDriverVehicleRequest> _dependencyCollection;
    
    public AddDriverVehicleEndpoint(BaseControllerParams<AddDriverVehicleRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("drivers/vehicles")]
    public async Task<ApiResponse<AddDriverVehicleResponse>> AddVehicle(
        [FromForm] AddDriverVehicleRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<AddDriverVehicleResponseDto, AddDriverVehicleResponse>(
                Result<AddDriverVehicleResponseDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        // Upload vehicle images if provided
        var imageUrls = new List<string>();
        if (request.Images != null && request.Images.Length > 0)
        {
            foreach (var image in request.Images)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(image, "vehicles"));
                if (uploadResult.IsSuccess)
                {
                    imageUrls.Add(uploadResult.Value);
                }
                else
                {
                    return HandleResult<AddDriverVehicleResponseDto, AddDriverVehicleResponse>(
                        Result<AddDriverVehicleResponseDto>.Failure(new Lines.Application.Shared.Error("UPLOAD_FAILED", uploadResult.Error.Description, Lines.Application.Shared.ErrorType.Validation)));
                }
            }
        }

        // Upload license photos (required)
        var licensePhotoUrls = new List<string>();
        if (request.LicensePhotos != null && request.LicensePhotos.Length > 0)
        {
            foreach (var licensePhoto in request.LicensePhotos)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(licensePhoto, "licenses"));
                if (uploadResult.IsSuccess)
                {
                    licensePhotoUrls.Add(uploadResult.Value);
                }
                else
                {
                    return HandleResult<AddDriverVehicleResponseDto, AddDriverVehicleResponse>(
                        Result<AddDriverVehicleResponseDto>.Failure(new Lines.Application.Shared.Error("UPLOAD_LICENSE_PHOTOS_FAILED", uploadResult.Error.Description, Lines.Application.Shared.ErrorType.Validation)));
                }
            }
        }

        var vehicleDto = new AddDriverVehicleDto
        {
            Name = request.Name,
            VehicleTypeId = request.VehicleTypeId,
            Model = request.Model,
            Year = request.Year,
            LicensePlate = request.LicensePlate,
            KmPrice = request.KmPrice,
            Color = request.Color,
            ImageUrls = imageUrls,
            LicensePhotoUrls = licensePhotoUrls,           // NEW
            LicenseNumber = request.LicenseNumber,          // NEW
            LicenseIssueDate = request.LicenseIssueDate,    // NEW
            LicenseExpiryDate = request.LicenseExpiryDate   // NEW
        };

        var result = await _mediator.Send(new AddDriverVehicleOrchestrator(userId, vehicleDto));
        return HandleResult<AddDriverVehicleResponseDto, AddDriverVehicleResponse>(result);
    }
}
