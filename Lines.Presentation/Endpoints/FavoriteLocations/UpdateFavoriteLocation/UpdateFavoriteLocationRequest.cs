using FluentValidation;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public record UpdateFavoriteLocationRequest(Guid Id, double Latitude, double Longtude, string Name, Guid PassengerId, Guid CityId, Guid SectorId);

public class UpdateFavoriteLocationRequestValidator : AbstractValidator<UpdateFavoriteLocationRequest>
{
    public UpdateFavoriteLocationRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Latitude).NotNull().NotEmpty();
        RuleFor(x => x.Longtude).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.PassengerId).NotNull().NotEmpty();
        RuleFor(x => x.CityId).NotNull().NotEmpty();
        RuleFor(x => x.SectorId).NotNull().NotEmpty();
        
    }
}