using Lines.Application.Shared;

namespace Lines.Application.Features.EmergencyNumbers.Shared;

public static class EmergencyNumberErrors
{
    public static Error EmergencyNumberExist => new Error("EmergencyNumbers.Exist", "This Emergency Number Already Exist for this User ", ErrorType.Validation);
    public static Error UserMismatch => new Error("EmergencyNumbers.Invalid user", "You can only edit your own emergency numbers ", ErrorType.Validation);
}