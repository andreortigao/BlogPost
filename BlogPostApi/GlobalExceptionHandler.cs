using BlogPostApplication.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostApi
{
    internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = exception switch
            {
                ValidationException ex => new ProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Validation errors",
                    Detail = ex.Message,
                    Extensions = new Dictionary<string, object?>
                    {
                        { "Errors", ex.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }) }
                    }
                },
                NotFoundException ex => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = ex.Message
                },
                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server error"
                }
            };

            httpContext.Response.StatusCode = problemDetails.Status!.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
