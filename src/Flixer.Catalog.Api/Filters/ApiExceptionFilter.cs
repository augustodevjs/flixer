using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Application.Exceptions;

namespace Flixer.Catalog.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;

        public ApiExceptionFilter(IHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var details = CreateProblemDetails(context.Exception);

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(details);
            context.HttpContext.Response.StatusCode = (int)details.Status!;
        }

        private ProblemDetails CreateProblemDetails(Exception exception)
        {
            var details = new ProblemDetails();

            if (_env.IsDevelopment())
            {
                details.Extensions.Add("StackTrace", exception.StackTrace);
            }

            SetProblemDetailsByExceptionType(details, exception);

            return details;
        }

        private static void SetProblemDetailsByExceptionType(ProblemDetails details, Exception exception)
        {
            details.Title = exception switch
            {
                EntityValidationException => "One or more validation errors occurred",
                NotFoundException => "Not Found",
                _ => "An unexpected error occurred"
            };

            details.Status = exception switch
            {
                EntityValidationException => StatusCodes.Status422UnprocessableEntity,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            details.Type = exception switch
            {
                EntityValidationException => "UnprocessableEntity",
                NotFoundException => "NotFound",
                _ => "UnexpectedError"
            };

            details.Detail = exception.Message;
        }
    }
}