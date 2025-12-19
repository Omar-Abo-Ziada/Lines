using FluentValidation;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverContact;

public class GetDriverContactRequest
{
    // Empty request - user ID comes from authentication
}

public class GetDriverContactRequestValidator : AbstractValidator<GetDriverContactRequest>
{
    public GetDriverContactRequestValidator()
    {
        // No validation needed for empty request
    }
}
