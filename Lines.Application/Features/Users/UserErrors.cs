namespace Lines.Application.Features.Users;

public static class UserErrors
{
    public static Error LoginUserError(string description)
    {
        return new Error("USER.LOGIN_ERROR", description, ErrorType.Failure);
    }

    public static Error RegisterUserError(string description)
    {
        return new Error("USER.REGISTER_ERROR", description, ErrorType.Failure);
    }
}
