using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record GetAllVehicleTypesRequest(string? Name , int? Capacity, int PageSize = 10, int PageNumber = 1);