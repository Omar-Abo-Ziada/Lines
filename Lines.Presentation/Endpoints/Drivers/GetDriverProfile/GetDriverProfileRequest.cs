using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverProfile;

public record GetDriverProfileRequest;

public class GetDriverProfileRequestValidator : AbstractValidator<GetDriverProfileRequest>
{
    public GetDriverProfileRequestValidator()
    {
        // No validation needed - uses authenticated user ID from token
    }
}
