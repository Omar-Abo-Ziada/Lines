using Lines.Application.Shared;

namespace Lines.Application.Features.Cities;

public class CityErrors
{
    public static Error CityNotExistError => new Error("CITY:NOTEXIST", "City Not exists", ErrorType.Validation);
}