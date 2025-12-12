using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodeOptimizer.Infrastructure.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = ex.Message;

        if (ex is Exceptions.CustomException customEx)
        {
            statusCode = customEx.StatusCode;
        }


        var exceptionResponse = new
        {
            TraceId = context.TraceIdentifier,
            StatusCode = (int)statusCode,
            Status = statusCode.ToString(),
            Message = message
        };

        var responseJson = JsonSerializer.Serialize(exceptionResponse, new JsonSerializerOptions { WriteIndented = true });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        _logger.LogError(ex, "An exception occurred: {@response}", responseJson);

        await context.Response.WriteAsync(responseJson);
    }
}
