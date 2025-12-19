using FluentValidation;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public record DeleteFavoriteLocationRequest(Guid Id);

public class DeleteFavoriteLocationRequestValidator : AbstractValidator<DeleteFavoriteLocationRequest>
{
    public DeleteFavoriteLocationRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();

    }
}