namespace Lines.Presentation.Endpoints.VehicleTypes.GetAllVehicleTypesWithExpectedPrice
{
    public record GetAllVehicleTypesWithExpectedPriceRequest(decimal Km ,Guid? UserRewardID);

    public class GetAllVehicleTypesWithExpectedPriceRequestValidator : AbstractValidator<GetAllVehicleTypesWithExpectedPriceRequest>
    {
        public GetAllVehicleTypesWithExpectedPriceRequestValidator()
        {
            RuleFor(x => x.Km)
            .GreaterThan(0).WithMessage("Km must be greater than zero");
        }
    }
}
