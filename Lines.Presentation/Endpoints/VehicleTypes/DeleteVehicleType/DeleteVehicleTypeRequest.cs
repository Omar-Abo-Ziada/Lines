using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record DeleteVehicleTypeRequest(Guid Id);

public class DeleteVehicleTypeRequestValidator : AbstractValidator<DeleteVehicleTypeRequest>
{
    public DeleteVehicleTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Vehicle Id is required.");
    }
}