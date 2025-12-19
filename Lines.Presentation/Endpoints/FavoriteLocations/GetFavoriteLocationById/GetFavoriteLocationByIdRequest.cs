using FluentValidation;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public record GetFavoriteLocationByIdRequest(Guid Id);
