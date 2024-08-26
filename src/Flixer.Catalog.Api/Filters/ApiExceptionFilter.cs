using Serilog.Context;
using Microsoft.AspNetCore.Mvc; 
using Flixer.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Flixer.Catalog.Application.Exceptions;

namespace Flixer.Catalog.Api.Filters;

    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(IHostEnvironment env, ILogger<ApiExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var httpContext = context.HttpContext;

            using (LogContext.PushProperty("RequestPath", httpContext.Request.Path))
            using (LogContext.PushProperty("RequestMethod", httpContext.Request.Method))
            {
                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred",
                    Type = "UnexpectedError",
                    Detail = exception.Message
                };

                switch (exception)
                {
                    case EntityValidationException validationException:
                        details.Title = "One or more validation errors occurred";
                        details.Status = StatusCodes.Status422UnprocessableEntity;
                        details.Type = "UnprocessableEntity";
                        details.Detail = validationException.Message;
                        details.Extensions.Add("Errors", validationException.Errors);

                        _logger.LogWarning(exception, "Validation error occurred. Details: {ValidationErrors}", validationException.Errors);
                        break;

                    case NotFoundException notFoundException:
                        details.Title = "Not Found";
                        details.Status = StatusCodes.Status404NotFound;
                        details.Type = "NotFound";
                        details.Detail = notFoundException.Message;

                        _logger.LogWarning(exception, "Resource not found. Details: {Message}", notFoundException.Message);
                        break;

                    default:
                        details.Title = "An unexpected error occurred";
                        details.Status = StatusCodes.Status500InternalServerError;
                        details.Type = "UnexpectedError";
                        details.Detail = "An unexpected error occurred. Please contact support.";

                        _logger.LogError(exception, "An unexpected error occurred. Request Path: {RequestPath}, Method: {RequestMethod}", httpContext.Request.Path, httpContext.Request.Method);
                        break;
                }

                if (_env.IsDevelopment())
                {
                    details.Extensions.Add("StackTrace", exception.StackTrace);
                    _logger.LogDebug("Stack trace: {StackTrace}", exception.StackTrace);
                }

                context.ExceptionHandled = true;
                context.Result = new ObjectResult(details)
                {
                    StatusCode = (int)details.Status
                };
                context.HttpContext.Response.StatusCode = (int)details.Status;
            }
        }
    }