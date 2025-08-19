using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.DTOs
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = [];
        public string? RequestId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public object? Meta { get; set; }

        // Basic constructors
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Message = message ?? GetDefaultMessage(statusCode);
            Data = null;

            if (!string.IsNullOrEmpty(message) && !IsSuccess)
            {
                Errors.Add(message);
            }
        }

        public ApiResponse(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Errors = errors ?? [];
            Message = GetDefaultMessage(statusCode);
        }

        public ApiResponse(int statusCode, object data)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Data = data;
            Message = GetDefaultMessage(statusCode);
        }

        public ApiResponse(int statusCode, object data, List<string> errors)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Data = data;
            Errors = errors ?? [];
            Message = GetDefaultMessage(statusCode);
        }

        public ApiResponse(int statusCode, ModelStateDictionary modelState)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Data = null;
            Message = GetDefaultMessage(statusCode);

            if (modelState != null && !modelState.IsValid)
            {
                Errors = [.. modelState
                .SelectMany(kvp => kvp.Value!.Errors
                    .Select(e => $"{kvp.Key}: {e.ErrorMessage}"))];
            }
        }

        // Advanced constructor with all parameters
        public ApiResponse(int statusCode, string? message, object? data, List<string>? errors, object? meta)
        {
            StatusCode = statusCode;
            IsSuccess = IsSuccessStatusCode(statusCode);
            Message = message ?? GetDefaultMessage(statusCode);
            Data = data;
            Errors = errors ?? [];
            Meta = meta;
        }

        private bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                422 => "Validation Error",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };
        }

        // Static helper methods for common responses
        public static ApiResponse CreateSuccess(string message = "Operation completed successfully")
        {
            return new ApiResponse(200, message);
        }

        public static ApiResponse CreateSuccess(object data, string message = "Operation completed successfully")
        {
            return new ApiResponse(200, data) { Message = message };
        }

        public static ApiResponse CreateCreated(object data, string message = "Resource created successfully")
        {
            return new ApiResponse(201, data) { Message = message };
        }

        public static ApiResponse CreateNoContent(string message = "No content")
        {
            return new ApiResponse(204, message);
        }

        public static ApiResponse CreateBadRequest(string message = "Bad request")
        {
            return new ApiResponse(400, message);
        }

        public static ApiResponse CreateBadRequest(List<string> errors)
        {
            return new ApiResponse(400, errors);
        }

        public static ApiResponse CreateBadRequest(ModelStateDictionary modelState)
        {
            return new ApiResponse(400, modelState);
        }

        public static ApiResponse CreateUnauthorized(string message = "Unauthorized access")
        {
            return new ApiResponse(401, message);
        }

        public static ApiResponse CreateForbidden(string message = "Access forbidden")
        {
            return new ApiResponse(403, message);
        }

        public static ApiResponse CreateNotFound(string message = "Resource not found")
        {
            return new ApiResponse(404, message);
        }

        public static ApiResponse CreateConflict(string message = "Resource conflict")
        {
            return new ApiResponse(409, message);
        }

        public static ApiResponse CreateValidationError(List<string> errors)
        {
            return new ApiResponse(422, errors);
        }

        public static ApiResponse CreateInternalServerError(string message = "Internal server error")
        {
            return new ApiResponse(500, message);
        }

        public static ApiResponse CreateCustom(int statusCode, string message)
        {
            return new ApiResponse(statusCode, message);
        }

        public static ApiResponse CreateCustom(int statusCode, object data, string message)
        {
            return new ApiResponse(statusCode, data) { Message = message };
        }

        // Method to add metadata
        public ApiResponse WithMeta(object meta)
        {
            Meta = meta;
            return this;
        }

        // Method to add request ID
        public ApiResponse WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = [];
        public int StatusCode { get; set; } = 200;
        public string? RequestId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public object? Meta { get; set; }

        // Basic constructors
        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = "", bool isSuccess = true)
        {
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = isSuccess ? 200 : 400;
        }

        public ApiResponse(string message, bool isSuccess, int statusCode = 400)
        {
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            if (!isSuccess && !string.IsNullOrEmpty(message))
            {
                Errors.Add(message);
            }
        }

        public ApiResponse(List<string> errors, int statusCode = 400)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Errors = errors ?? [];
            Message = GetDefaultMessage(statusCode);
        }

        // Advanced constructor
        public ApiResponse(T? data, string message, bool isSuccess, int statusCode, List<string>? errors = null, object? meta = null)
        {
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Errors = errors ?? [];
            Meta = meta;
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                422 => "Validation Error",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };
        }

        // Static helper methods for common responses
        public static ApiResponse<T> CreateSuccess(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = 200,
                Errors = []
            };
        }

        public static ApiResponse<T> CreateCreated(T data, string message = "Resource created successfully")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = 201,
                Errors = []
            };
        }

        public static ApiResponse<T> CreateNoContent(string message = "No content")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = default(T),
                StatusCode = 204,
                Errors = []
            };
        }

        public static ApiResponse<T> CreateBadRequest(string message)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 400,
                Errors = string.IsNullOrEmpty(message) ? [] : [message]
            };
        }

        public static ApiResponse<T> CreateBadRequest(List<string> errors)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = "Bad Request",
                Data = default(T),
                StatusCode = 400,
                Errors = errors ?? []
            };
        }

        public static ApiResponse<T> CreateBadRequest(ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            if (modelState != null && !modelState.IsValid)
            {
                errors = [.. modelState
                .SelectMany(kvp => kvp.Value!.Errors
                    .Select(e => $"{kvp.Key}: {e.ErrorMessage}"))];
            }

            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = "Validation Error",
                Data = default(T),
                StatusCode = 400,
                Errors = errors
            };
        }

        public static ApiResponse<T> CreateUnauthorized(string message = "Unauthorized access")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 401,
                Errors = [message]
            };
        }

        public static ApiResponse<T> CreateForbidden(string message = "Access forbidden")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 403,
                Errors = [message]
            };
        }

        public static ApiResponse<T> CreateNotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 404,
                Errors = [message]
            };
        }

        public static ApiResponse<T> CreateConflict(string message = "Resource conflict")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 409,
                Errors = [message]
            };
        }

        public static ApiResponse<T> CreateValidationError(List<string> errors)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = "Validation Error",
                Data = default(T),
                StatusCode = 422,
                Errors = errors ?? []
            };
        }

        public static ApiResponse<T> CreateInternalServerError(string message = "Internal server error")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T),
                StatusCode = 500,
                Errors = [message]
            };
        }

        public static ApiResponse<T> CreateCustom(int statusCode, string message, T? data = default(T))
        {
            return new ApiResponse<T>
            {
                IsSuccess = statusCode >= 200 && statusCode <= 299,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                Errors = statusCode >= 200 && statusCode <= 299 ? [] : [message]
            };
        }

        // Method to add metadata
        public ApiResponse<T> WithMeta(object meta)
        {
            Meta = meta;
            return this;
        }

        // Method to add request ID
        public ApiResponse<T> WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }

 
    }

    // Extension methods for easier usage
    public static class ApiResponseExtensions
    {
        public static ApiResponse<T> ToApiResponse<T>(this T data, string message = "Success")
        {
            return ApiResponse<T>.CreateSuccess(data, message);
        }

        public static ApiResponse<T> ToErrorResponse<T>(this string errorMessage, int statusCode = 400)
        {
            return ApiResponse<T>.CreateCustom(statusCode, errorMessage);
        }

        public static ApiResponse<T> ToValidationErrorResponse<T>(this ModelStateDictionary modelState)
        {
            return ApiResponse<T>.CreateBadRequest(modelState);
        }

        // Add these new extension methods
        public static IActionResult ToActionResult(this ApiResponse apiResponse)
        {
            return new ObjectResult(apiResponse) { StatusCode = apiResponse.StatusCode };
        }

        public static IActionResult ToActionResult<T>(this ApiResponse<T> apiResponse)
        {
            return new ObjectResult(apiResponse) { StatusCode = apiResponse.StatusCode };
        }
    }
}