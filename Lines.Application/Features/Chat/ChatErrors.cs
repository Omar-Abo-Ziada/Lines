using Lines.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Chat
{
    public static class ChatErrors
    {
        public static Error NonEligobleChattingPart(string desc) => new Error("SendMessageError", desc, ErrorType.Validation);
        public static Error TripDoesNotExist(string desc) => new Error("TripNotFound", desc, ErrorType.Validation);
        public static Error InvalidTripId(string desc) => new Error("InvalidTripId", desc, ErrorType.Validation);
        public static Error InvalidUserId(string desc) => new Error("InvalidUserId", desc, ErrorType.Validation);
        public static Error InvalidPageNumber(string desc) => new Error("InvalidPageNumber", desc, ErrorType.Validation);
        public static Error InvalidPageSize(string desc) => new Error("InvalidPageSize", desc, ErrorType.Validation);
        public static Error FailedToUploadImage(string desc) => new Error("FailedToUploadImage", desc, ErrorType.Validation);
        public static Error InvalidImage(string desc) => new Error("InvalidImage", desc, ErrorType.Validation);
    }
}
