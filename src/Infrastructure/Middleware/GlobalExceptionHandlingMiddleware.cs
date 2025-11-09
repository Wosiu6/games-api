using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Application.Common.Exceptions;

namespace Infrastructure.Middleware;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = GetErrorResponse(exception);
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                message,
                statusCode = (int)statusCode,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
        };

        var jsonResponse = JsonSerializer.Serialize(response, _jsonSerializerOptions);

        await context.Response.WriteAsync(jsonResponse);
    }

    private static (HttpStatusCode statusCode, string message) GetErrorResponse(Exception exception)
    {
        return exception switch
        {
            ValidationException validationEx => (HttpStatusCode.BadRequest, string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))),
            EmailAlreadyInUseException => (HttpStatusCode.Conflict, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Access denied"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            TimeoutException => (HttpStatusCode.RequestTimeout, "Request timeout"),
            NotImplementedException => (HttpStatusCode.NotImplemented, "Feature not implemented"),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred")
        };
    }
}