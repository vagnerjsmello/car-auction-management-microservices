using CAMS.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
namespace CAMS.Common.Middleware;


public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Continua no pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            VehicleAlreadyExistsException or InvalidBidException or AuctionAlreadyActiveException => (int)HttpStatusCode.BadRequest,
            VehicleNotFoundException or AuctionNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var errors = exception switch
        {
            ValidationException validationEx => validationEx.Errors.Select(e => e.ErrorMessage).ToList(),
            _ => new List<string> { exception.Message ?? "An unexpected error occurred." }
        };

        var response = new
        {
            IsSuccess = false,
            Data = (string?)null,
            Errors = errors
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonConvert.SerializeObject(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

