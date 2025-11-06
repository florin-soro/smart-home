namespace Thermostat.Api.Middlewares
{
    public class RequestBodyLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                var requestBody = await reader.ReadToEndAsync();
                context.Items["RequestBody"] = requestBody;
            }

            context.Request.Body.Position = 0;

            await _next(context);
        }
    }
}
