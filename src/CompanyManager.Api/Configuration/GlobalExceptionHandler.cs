using Microsoft.AspNetCore.Mvc;
using System.Net;
using ILogger = Serilog.ILogger;

namespace CompanyManager.Api.Configuration;

public class GlobalExceptionHandler
{
    private readonly HttpContext _context;
    private readonly Exception _exception;
    private readonly ILogger _logger;

    public GlobalExceptionHandler(HttpContext context, Exception exception, ILogger logger)
    {
        _context = context;
        _exception = exception;
        _logger = logger;
    }

    public GlobalExceptionHandler PrepareErrorResponse()
    {
        _context.Response.Clear();
        _context.Response.ContentType = "application/json";
        return this;
    }

    public GlobalExceptionHandler LogException()
    {
        _logger.Error(_exception, "Global error handler: {0}", _exception.Message);

        return this;
    }

    public ProblemDetails CreateProblemDetails()
    {
        switch (_exception)
        {
            case ArgumentNullException _:
            case ArgumentException _:
            case InvalidOperationException _:
            {
                const int statusCode = (int)HttpStatusCode.BadRequest;
                _context.Response.StatusCode = statusCode;

                return new ProblemDetails
                {
                    Title = "Invalid argument",
                    Status = statusCode,
                    Detail = _exception.Message,
                    Type = _exception.GetType().ToString()
                };
            }
            case HttpRequestException _:
            {
                const int statusCode = (int)HttpStatusCode.BadRequest;
                _context.Response.StatusCode = statusCode;

                return new ProblemDetails
                {
                    Title = "Invalid HTTP request",
                    Status = statusCode,
                    Detail = _exception.Message,
                    Type = _exception.GetType().ToString()
                };
            }

            default:
            {
                const int statusCode = (int)HttpStatusCode.InternalServerError;
                _context.Response.StatusCode = statusCode;

                return new ProblemDetails
                {
                    Title = "Something went wrong",
                    Status = statusCode,
                    Detail = _exception.InnerException?.Message ?? _exception.Message,
                    Type = _exception.GetType().ToString()
                };
            }
        }
    }
}