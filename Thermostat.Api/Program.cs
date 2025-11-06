using Serilog;
using Thermostat.Api.Middlewares;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup();

        startup.ConfigureServices(builder.Services);

        builder.Host.UseSerilog();
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(int.Parse(port)); // listen on all network interfaces
        });
        var app = builder.Build();
        app.UseMiddleware<FinalExceptionHandlerMiddleware>(app.Environment);
        app.UseMiddleware<RequestBodyLoggingMiddleware>();
        startup.Configure(app, app.Environment);

        app.Run();
    }
}
