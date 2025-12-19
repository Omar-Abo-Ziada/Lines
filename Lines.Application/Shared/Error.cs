namespace Lines.Application.Shared;

public enum ErrorType { Failure, Validation, NotFound, Conflict, UnAuthorized, NotAuthenticated }

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result is null.", ErrorType.Failure);
    public static readonly Error General = new("Error.General", "Something went Wrong, Please try Again later.", ErrorType.Failure);
    public static readonly Error Validation = new("Error.Validation", "The specified result is invalid.", ErrorType.Validation);
    public static readonly Error NotFound = new("Error.NotFound", "The specified result is not found.", ErrorType.NotFound);
    public static readonly Error Conflict = new("Error.Conflict", "The specified result is already exist.", ErrorType.Conflict);
    public static readonly Error UnAuthorized = new("Error.UnAuthorized", "The specified result is not authorized.", ErrorType.Failure);


     public static Error Create(string code, string description, ErrorType type = ErrorType.Failure)
        => new(code, description, type);
}