namespace Application.Common;

using System;
using System.Collections.Generic;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(bool success, string message, T? data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public ApiResponse(bool success, string message, List<string> errors)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(false, message, errors ?? new List<string>());
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public List<string>? Errors { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(bool success, string message, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }

    public static ApiResponse SuccessResponse(string message = "Success")
    {
        return new ApiResponse(true, message);
    }

    public static ApiResponse ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse(false, message, errors ?? new List<string>());
    }
}
