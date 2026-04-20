namespace API.Middleware;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Application.Common;
using Application.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError($"An exception occurred: {exception.Message}");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ApiResponse response = null!;

        switch (exception)
        {
            case ValidationException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = ApiResponse.ErrorResponse("Validasyon hatası", ex.Errors);
                break;

            case UnauthorizedException ex:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = ApiResponse.ErrorResponse(ex.Message);
                break;

            case NotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = ApiResponse.ErrorResponse(ex.Message);
                break;

            case BadRequestException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = ApiResponse.ErrorResponse(ex.Message);
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = ApiResponse.ErrorResponse("Beklenmeyen bir hata oluştu");
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
