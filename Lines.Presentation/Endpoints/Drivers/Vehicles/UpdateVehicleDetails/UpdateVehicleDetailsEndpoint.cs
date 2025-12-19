using Lines.Application.Features.Vehicles.UpdateVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.UpdateVehicleDetails.Orchestrators;
using Lines.Application.Features.Common.Commands;
using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.UpdateVehicleDetails;

public class UpdateVehicleDetailsEndpoint : BaseController<UpdateVehicleDetailsRequest, UpdateVehicleDetailsResponse>
{
    private readonly BaseControllerParams<UpdateVehicleDetailsRequest> _dependencyCollection;
    
    public UpdateVehicleDetailsEndpoint(BaseControllerParams<UpdateVehicleDetailsRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("drivers/update-vehicles/{vehicleId}")]
    public async Task<ApiResponse<UpdateVehicleDetailsResponse>> UpdateVehicleDetails(
        [FromRoute] Guid vehicleId,
        [FromForm] UpdateVehicleDetailsRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<VehicleDetailsDto, UpdateVehicleDetailsResponse>(
                Result<VehicleDetailsDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
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
                    return HandleResult<VehicleDetailsDto, UpdateVehicleDetailsResponse>(
                        Result<VehicleDetailsDto>.Failure(new Lines.Application.Shared.Error("UPLOAD_FAILED", uploadResult.Error.Description, Lines.Application.Shared.ErrorType.Validation)));
                }
            }
        }

        // Upload license photos if provided
        var licensePhotoUrls = new List<string>();
        if (request.LicensePhotos != null && request.LicensePhotos.Length > 0)
        {
            foreach (var photo in request.LicensePhotos)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(photo, "licenses"));
                if (uploadResult.IsSuccess)
                {
                    licensePhotoUrls.Add(uploadResult.Value);
                }
                else
                {
                    return HandleResult<VehicleDetailsDto, UpdateVehicleDetailsResponse>(
                        Result<VehicleDetailsDto>.Failure(new Lines.Application.Shared.Error("UPLOAD_FAILED", uploadResult.Error.Description, Lines.Application.Shared.ErrorType.Validation)));
                }
            }
        }

        // Upload registration documents if provided
        var registrationDocumentUrls = new List<string>();
        if (request.RegistrationDocuments != null && request.RegistrationDocuments.Length > 0)
        {
            foreach (var document in request.RegistrationDocuments)
            {
                var uploadResult = await _mediator.Send(new UploadImageOrchestrator(document, "registration"));
                if (uploadResult.IsSuccess)
                {
                    registrationDocumentUrls.Add(uploadResult.Value);
                }
                else
                {
                    return HandleResult<VehicleDetailsDto, UpdateVehicleDetailsResponse>(
                        Result<VehicleDetailsDto>.Failure(new Lines.Application.Shared.Error("UPLOAD_FAILED", uploadResult.Error.Description, Lines.Application.Shared.ErrorType.Validation)));
                }
            }
        }

        var updateDto = new UpdateVehicleDetailsDto
        {
            Name = request.Name,
            Model = request.Model,
            Year = request.Year,
            Color = request.Color,
            KmPrice = request.KmPrice,
            LicensePlate = request.LicensePlate,
            VehicleTypeId = request.VehicleTypeId,
            ImageUrls = imageUrls,
            LicensePhotoUrls = licensePhotoUrls,
            RegistrationDocumentUrls = registrationDocumentUrls
        };

        var result = await _mediator.Send(new UpdateVehicleDetailsOrchestrator(vehicleId, userId, updateDto));
        return HandleResult<VehicleDetailsDto, UpdateVehicleDetailsResponse>(result);
    }
}
