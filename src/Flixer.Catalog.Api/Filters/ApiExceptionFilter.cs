using Microsoft.AspNetCore.Mvc; 
using Flixer.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Flixer.Catalog.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ApiExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if(_env.IsDevelopment())
        {
            details.Extensions.Add("StackTrace", exception.StackTrace);
        }

        if (exception is EntityValidationException)
        {
            var ex = exception as EntityValidationException;

            details.Title = "One or more validation errors ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = ex!.Message;
        } else
        {
            details.Title = "An unexpected error ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnexpectedError";
            details.Detail = exception.Message;
        }

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(details);
        context.HttpContext.Response.StatusCode = (int) details.Status;
    }
}