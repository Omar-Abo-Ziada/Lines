using Lines.Application.Shared;

namespace Lines.Application.Features.Examples;

public static class ExampleErrors
{
    public static Error ExampleExist => new Error("Example.Exist", "An example with the same name already exists.",ErrorType.Validation);
}