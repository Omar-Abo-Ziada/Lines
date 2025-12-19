using FluentValidation;

namespace Lines.Presentation.Endpoints.VehicleTypes;

public record GetVehicleTypesListRequest(double Latitude, double Longitude);