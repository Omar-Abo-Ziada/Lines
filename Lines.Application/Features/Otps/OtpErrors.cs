using Lines.Application.Shared;

namespace Lines.Application.Features.Otps
{
    public static class OtpErrors
    {
        public static Error InvalidOtpError(string desc) => new Error("OTP.Invalid_error", desc, ErrorType.Validation);
    }
}
