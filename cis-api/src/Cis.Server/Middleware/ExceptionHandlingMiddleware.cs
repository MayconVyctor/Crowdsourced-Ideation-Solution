using System.Text.Json;
using Cis.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cis.Server.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CisValidationException ex)
        {
            logger.LogWarning(ex, "Validation exception caught: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (TopicNotFoundException ex)
        {
            logger.LogWarning(ex, "Topic not found: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                StatusCodes.Status404NotFound,
                "Not Found",
                ex.Message);
        }
        catch (TopicForbiddenException ex)
        {
            logger.LogWarning(ex, "Topic forbidden: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                StatusCodes.Status403Forbidden,
                "Forbidden",
                ex.Message);
        }
        catch (TopicClosedException ex)
        {
            logger.LogWarning(ex, "Topic closed: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                StatusCodes.Status409Conflict,
                "Conflict",
                ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex, "User not found: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                StatusCodes.Status422UnprocessableEntity,
                "Unprocessable Entity",
                ex.Message);
        }
        catch (CisUnprocessableEntityException ex)
        {
            logger.LogWarning(ex, "Unprocessable entity caught: {Message}", ex.Message);
            await HandleExceptionAsync(
                context,
                StatusCodes.Status422UnprocessableEntity,
                "Unprocessable Entity",
                ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception caught: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string error, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorResponse = new ErrorResponse(statusCode, error, message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
    }
}
