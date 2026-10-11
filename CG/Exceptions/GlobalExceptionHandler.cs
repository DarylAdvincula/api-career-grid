using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CG.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken token
        )
        {
            ProblemDetails problem;

            if (exception is AppException appException)
            {
                _logger.LogWarning(
                    "{Type}: {Message}",
                    appException.GetType().Name,
                    appException.Message
                );

                problem = new ProblemDetails
                {
                    Status = appException.StatusCode,
                    Title = appException.Title,
                    Detail = appException.Message
                };
            }
            else
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception on {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path
                );

                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred."
                };
            }

            context.Response.StatusCode = problem.Status!.Value;
            await context.Response.WriteAsJsonAsync(problem, token);
            return true;
        }
    }
}