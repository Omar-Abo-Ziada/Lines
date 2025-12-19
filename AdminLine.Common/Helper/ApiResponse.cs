using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLine.Common.Helper
{
    public record ApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public Error? Error { get; init; }
        public int StatusCode { get; init; }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200)
            => new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static ApiResponse<T> ErrorResponse(Error error, int statusCode)
            => new() { IsSuccess = false, Error = error, StatusCode = statusCode };
    }
}
