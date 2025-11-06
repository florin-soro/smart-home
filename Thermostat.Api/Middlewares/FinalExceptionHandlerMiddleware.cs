using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Thermostat.Api.Middlewares
{
    public class FinalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FinalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public FinalExceptionHandlerMiddleware(RequestDelegate next, ILogger<FinalExceptionHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A critical unhandled exception occurred: {Message}", ex.Message);

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "An internal server error occurred.",
                    Detail = _env.IsDevelopment() ? ex.ToString() : "An unexpected error prevented the request from completing.",
                    Instance = context.Request.Path
                };

                problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

                var jsonResponse = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
