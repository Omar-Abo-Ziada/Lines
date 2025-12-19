# Update Vehicle Details Endpoint Implementation Plan

## Overview

Create endpoint for updating vehicle details, allowing drivers to edit their vehicle information such as name, model, year, color, KM price, and upload/update photos and registration documents.

## Endpoint to Create

### PUT/PATCH Update Vehicle Details

**Route:** `PUT /api/drivers/vehicles/{vehicleId}` or `PATCH /api/drivers/vehicles/{vehicleId}`

**Request Fields (based on Figma vehicle details screen):**

- Name (vehicle nickname) - editable
- Model - editable
- Year - editable
- Color - editable
- KmPrice - editable
- LicensePlate - editable (may require admin verification)
- VehicleTypeId - editable (may require admin verification)
- Images (IFormFile[] - optional) - if provided, replaces all existing vehicle photos
- LicensePhotos (IFormFile[] - optional) - if provided, replaces all existing vehicle license photos
- RegistrationDocuments (IFormFile[] - optional) - if provided, replaces all existing registration documents

**Business Rules:**

1. Only vehicle owner can update their vehicle
2. Some changes may reset verification status (e.g., changing vehicle type, license plate)
3. If Images provided → remove all existing vehicle photos and replace with new ones (first photo becomes primary)
4. If LicensePhotos provided → remove all existing vehicle license photos and replace with new ones
5. If RegistrationDocuments provided → remove all existing registration documents and replace with new ones
6. Cannot update vehicle if it's currently in an active trip
7. Changes to critical fields (VehicleTypeId, LicensePlate, LicensePhotos) may set Status to PendingVerification

**Response:**

- Updated vehicle details (same as GetVehicleDetails response)
- Success message

## Architecture Layers

### Application Layer

**Location:** `Lines.Application/Features/Vehicles/UpdateVehicleDetails/`

#### Update Vehicle Details

1. **Command:** `UpdateVehicleDetailsCommand.cs`

   - Takes vehicleId, userId, and update DTO
   - Verifies vehicle ownership
   - Validates business rules (not in active trip, etc.)
   - Updates vehicle entity using domain methods
   - Handles photo uploads/deletions
   - Handles registration document uploads/deletions
   - Resets verification if critical fields changed
   - Saves changes
   - Returns updated vehicle details

2. **Orchestrator:** `UpdateVehicleDetailsOrchestrator.cs`

   - Coordinates command execution
   - Returns Result<VehicleDetailsDto>

3. **DTO:** `UpdateVehicleDetailsDto.cs`
   - Input fields for update
   - List of image URLs (uploaded via separate process)
   - List of registration document URLs
   - Optional fields (only update provided fields)

### Presentation Layer

**Location:** `Lines.Presentation/Endpoints/Drivers/Vehicles/UpdateVehicleDetails/`

#### UpdateVehicleDetails

1. **Endpoint:** `UpdateVehicleDetailsEndpoint.cs`

   - Route: `[HttpPut("drivers/vehicles/{vehicleId}")]`
   - Uses `GetCurrentUserId()` for authentication
   - Validates vehicleId and request
   - Handles file uploads (images and documents)
   - Calls UpdateVehicleDetailsOrchestrator

2. **Request:** `UpdateVehicleDetailsRequest.cs`

   - All editable vehicle fields (optional)
   - Images (IFormFile[] - optional) - replaces all existing vehicle photos if provided
   - LicensePhotos (IFormFile[] - optional) - replaces all existing vehicle license photos if provided
   - RegistrationDocuments (IFormFile[] - optional) - replaces all existing registration documents if provided
   - FluentValidation rules

3. **Response:** `UpdateVehicleDetailsResponse.cs`
   - Same structure as GetVehicleDetailsResponse
   - Updated vehicle details

## Key Implementation Details

1. **Authorization:** Verify vehicle belongs to authenticated driver before update
2. **Partial Updates:** Only update fields that are provided (non-null)
3. **File Upload:**
   - Upload new images using `UploadImageOrchestrator`
   - Upload new registration documents
   - Store in appropriate folders ("vehicles", "registration")
4. **Photo Management:**
   - If Images provided → remove all existing VehiclePhoto entities
   - Upload new vehicle images
   - Set first uploaded image as primary
   - If Images NOT provided → keep existing vehicle photos unchanged
5. **License Photo Management:**
   - If LicensePhotos provided → remove all existing VehicleLicense.Photos (LicensePhoto entities)
   - Upload new license photos
   - Add new LicensePhoto entities to VehicleLicense
   - If LicensePhotos NOT provided → keep existing license photos unchanged
6. **Document Management:**
   - If RegistrationDocuments provided → clear existing documents list
   - Upload new registration documents
   - Serialize new document URLs to JSON
   - If RegistrationDocuments NOT provided → keep existing documents unchanged
7. **Verification Reset:**
   - If VehicleTypeId changes → set Status to PendingVerification, IsVerified = false
   - If LicensePlate changes → set Status to PendingVerification, IsVerified = false
   - If LicensePhotos provided (license updated) → set Status to PendingVerification, IsVerified = false
8. **Domain Methods:**
   - Use `vehicle.UpdateDetails(model, year)` for basic updates
   - Use `vehicle.UpdateKmPrice(kmPrice)` for price updates
   - Use `vehicle.AddPhoto(photo)` for new vehicle photos
   - Use `vehicle.License.AddPhoto(photo)` for new license photos
   - Use `vehicle.UpdateRegistrationDocuments(urls)` for documents
9. **Response:** Return full vehicle details after update (reuse GetVehicleDetailsQuery)

## Files to Create

### Application Layer (3 files)

1. `Lines.Application/Features/Vehicles/UpdateVehicleDetails/Commands/UpdateVehicleDetailsCommand.cs`
2. `Lines.Application/Features/Vehicles/UpdateVehicleDetails/Orchestrators/UpdateVehicleDetailsOrchestrator.cs`
3. `Lines.Application/Features/Vehicles/UpdateVehicleDetails/DTOs/UpdateVehicleDetailsDto.cs`

### Presentation Layer (3 files)

4. `Lines.Presentation/Endpoints/Drivers/Vehicles/UpdateVehicleDetails/UpdateVehicleDetailsEndpoint.cs`
5. `Lines.Presentation/Endpoints/Drivers/Vehicles/UpdateVehicleDetails/UpdateVehicleDetailsRequest.cs`
6. `Lines.Presentation/Endpoints/Drivers/Vehicles/UpdateVehicleDetails/UpdateVehicleDetailsResponse.cs`

## Pattern References

- Command pattern: `AddDriverVehicleCommand.cs`
- File upload: `AddDriverVehicleEndpoint.cs` (using UploadImageOrchestrator)
- Endpoint pattern: `GetDriverProfileEndpoint.cs`
- Authentication: BaseController's `GetCurrentUserId()` method
- Authorization: Verify driver owns the vehicle before operations
- Return full details: Reuse `GetVehicleDetailsQuery` after update

## Request/Response Structure

### UpdateVehicleDetails Request (multipart/form-data)

```
PUT /api/drivers/vehicles/{vehicleId}
Content-Type: multipart/form-data

Form Data:
- Name: "My Updated Car" (optional)
- Model: "Camry 2024" (optional)
- Year: 2024 (optional)
- Color: "Blue" (optional)
- KmPrice: 3.00 (optional)
- LicensePlate: "ABC123" (optional)
- VehicleTypeId: "guid" (optional)
- Images: [file1.jpg, file2.png] (optional - replaces all existing vehicle photos)
- LicensePhotos: [license1.jpg, license2.jpg] (optional - replaces all existing vehicle license photos)
- RegistrationDocuments: [doc1.pdf, doc2.jpg] (optional - replaces all existing registration documents)
```

### UpdateVehicleDetails Response

```json
{
  "vehicleId": "guid",
  "name": "My Updated Car",
  "licensePlate": "ABC123",
  "model": "Camry 2024",
  "year": 2024,
  "brand": "Toyota",
  "color": "Blue",
  "kmPrice": 3.0,
  "isActive": true,
  "isPrimary": true,
  "isVerified": false,
  "status": "PendingVerification",
  "vehiclePhotos": [
    {
      "photoUrl": "uploads/vehicles/new-photo.jpg",
      "description": null,
      "isPrimary": true
    }
  ],
  "licenseInfo": {
    "licenseNumber": "LIC123456",
    "issueDate": "2023-01-15",
    "expiryDate": "2025-01-15",
    "isValid": true,
    "photos": ["uploads/licenses/lic.jpg"]
  },
  "registrationDocuments": ["uploads/registration/new-doc.pdf"],
  "vehicleType": {
    "id": "guid",
    "name": "Sedan",
    "iconUrl": null
  }
}
```

## Update Logic Flow

1. **Authenticate & Authorize:**

   - Get userId from token
   - Resolve driverId
   - Verify vehicle belongs to driver

2. **Validate Business Rules:**

   - Check vehicle is not in active trip
   - Validate field values (year range, price > 0, etc.)

3. **Handle Vehicle Photo Updates:**

   - If Images provided:
     - Remove all existing VehiclePhoto entities from database
     - Upload new vehicle photos using UploadImageOrchestrator (folder: "vehicles")
     - Add new VehiclePhoto entities (first one as primary)
   - If Images NOT provided: Skip, keep existing vehicle photos

4. **Handle License Photo Updates:**

   - If LicensePhotos provided:
     - Remove all existing LicensePhoto entities from VehicleLicense
     - Upload new license photos using UploadImageOrchestrator (folder: "licenses")
     - Add new LicensePhoto entities to VehicleLicense
     - Set Status to PendingVerification (license updated)
   - If LicensePhotos NOT provided: Skip, keep existing license photos

5. **Handle Document Updates:**

   - If RegistrationDocuments provided:
     - Upload new documents using UploadImageOrchestrator (folder: "registration")
     - Create new document URLs list
     - Serialize to JSON and update RegistrationDocumentUrls
   - If RegistrationDocuments NOT provided: Skip, keep existing documents

6. **Update Vehicle Fields:**

   - Use domain methods for updates
   - Track if critical fields changed (VehicleTypeId, LicensePlate, LicensePhotos)
   - Reset verification if needed

7. **Save & Return:**
   - Save changes to database
   - Fetch updated vehicle details using GetVehicleDetailsQuery
   - Return full vehicle details

## Validation Rules

### UpdateVehicleDetailsRequest Validation

```csharp
- Name: MaxLength(100) if provided
- Model: MaxLength(100) if provided
- Year: Between(1900, CurrentYear + 2) if provided
- Color: MaxLength(50) if provided
- KmPrice: GreaterThan(0) if provided
- LicensePlate: MaxLength(20) if provided
- VehicleTypeId: NotEmpty if provided
- Images (vehicle photos):
  - Max 5 new images per request
  - Each file max 5MB
  - Only JPEG, PNG allowed
- LicensePhotos (vehicle license photos):
  - Max 3 new license photos per request
  - Each file max 5MB
  - Only JPEG, PNG allowed
- RegistrationDocuments:
  - Max 3 new documents per request
  - Each file max 10MB
  - JPEG, PNG, PDF allowed
```

## Domain Method Usage

The command will use existing Vehicle domain methods:

```csharp
// Update basic details
vehicle.UpdateDetails(model, year);

// Update price
vehicle.UpdateKmPrice(kmPrice);

// Add vehicle photos
vehicle.AddPhoto(new VehiclePhoto(vehicle, photoUrl, isPrimary));

// Add license photos
vehicle.License.AddPhoto(new LicensePhoto(vehicle.License, photoUrl));

// Update registration documents
vehicle.UpdateRegistrationDocuments(documentUrls);

// Reset verification if critical fields changed
if (criticalFieldsChanged)
{
    vehicle.SetStatus(VehicleRequestStatus.PendingVerification);
    vehicle.IsVerified = false;
}
```

## Additional Considerations

1. **Concurrency:** Consider optimistic concurrency to prevent conflicting updates
2. **Audit Trail:** Log what fields were changed (optional)
3. **Notifications:** May need to notify admin if critical fields change requiring re-verification
4. **Rollback:** If file upload fails partway through, clean up uploaded files
5. **Photo Replacement:** When replacing vehicle photos, old photo files remain on disk (consider cleanup job)
6. **License Photo Replacement:** When replacing license photos, old license photo files remain on disk (consider cleanup job)
7. **Document Replacement:** When replacing documents, old document files remain on disk (consider cleanup job)
