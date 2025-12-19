using FluentValidation;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public record GetAllFavoriteLocationsRequest(string? Name);
