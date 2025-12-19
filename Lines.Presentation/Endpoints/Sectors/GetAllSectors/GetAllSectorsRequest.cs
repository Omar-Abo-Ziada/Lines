using FluentValidation;

namespace Lines.Presentation.Endpoints.Sectors;

public record GetAllSectorsRequest(string? Name, Guid? CityId, int PageSize = 10, int PageNumber = 1);
