using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record GetVehicleTypeByIdRequest(Guid Id);

public class GetVehicleTypeByIdRequestValidator : AbstractValidator<GetVehicleTypeByIdRequest>
{
    public GetVehicleTypeByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("id is required");
    }
}