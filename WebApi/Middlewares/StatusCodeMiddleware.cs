using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WebApi.Dtos;
using WebApi.Exceptions;

namespace WebApi.Middlewares;

public class StatusCodeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StatusCodeMiddleware> _logger;

    public StatusCodeMiddleware(RequestDelegate next, ILogger<StatusCodeMiddleware> logger)
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
        catch (ServiceException e)
        {
            _logger.LogWarning("Validation error: {0}", e);

            await SetErrorBody(context, e.GetStatusCode(), e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("General error: {0}", e);

            await SetErrorBody(context, HttpStatusCode.InternalServerError, "General error");
        }
    }

    private static async Task SetErrorBody(HttpContext context, HttpStatusCode statusCode, string message)
    {
        var responseBody = JsonSerializer.Serialize(new ErrorDto { Message = message });

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        await context.Response.WriteAsync(responseBody, Encoding.UTF8);
    }
}
