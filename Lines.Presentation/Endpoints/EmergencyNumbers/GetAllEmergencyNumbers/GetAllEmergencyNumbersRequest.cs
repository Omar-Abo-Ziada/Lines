namespace Lines.Presentation.Endpoints.EmergencyNumbers;

public record GetAllEmergencyNumbersRequest(string? Name, string? PhoneNumber, int? EmergencyNumberType, int PageNumber = 1, int PageSize= 10);

