using FluentValidation;

namespace Lines.Presentation.Endpoints.Cities;

public record GetAllCitiesRequest(string? Name , int PageSize = 10, int PageNumber = 1);