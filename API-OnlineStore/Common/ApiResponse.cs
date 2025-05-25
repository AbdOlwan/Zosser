
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_OnlineStore.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public object? Data { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Success = IsSuccessStatusCode(statusCode);
            Data = null;

            if (!string.IsNullOrEmpty(message))
            {
                Errors.Add(message);
            }
        }

        public ApiResponse(int statusCode, ModelStateDictionary modelState)
        {
            StatusCode = statusCode;
            Success = IsSuccessStatusCode(statusCode);
            Data = null;

            if (modelState != null && !modelState.IsValid)
            {
                foreach (var key in modelState.Keys)
                {
                    var errors = modelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Errors.Add($"{key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
        }

        public ApiResponse(int statusCode, object data)
        {
            StatusCode = statusCode;
            Success = IsSuccessStatusCode(statusCode);
            Data = data;
        }

        private bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }
    }
}













namespace API_OnlineStore.Common
{
    public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
}

}
