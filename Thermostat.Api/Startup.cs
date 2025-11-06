using Azure.Identity;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Thermostat.Api.Config;
using Thermostat.Application;
using Thermostat.Domain.Extensions;
using Thermostat.DataAccessLayer;
using Thermostat.Api.Extensions;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup()
    {
        _configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddAzureAppConfiguration(options =>
            {
                options.Connect(
                new Uri("<...?>"),
                new ManagedIdentityCredential()
                   )
                        .ConfigureRefresh(refresh =>
                        {
                            refresh.Register("ConnectionStrings:SqlDB", refreshAll: true)
                                    .SetCacheExpiration(TimeSpan.FromSeconds(30));
                        });
            })
            .Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .CreateLogger();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_configuration);
        services.AddSettingsSection<IThermostatSettings, ThermostatSettings>(_configuration, nameof(ThermostatSettings));

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.ReturnHttpNotAcceptable = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Thermostat API",
                Description = "API documentation for the Thermostat system"
            });
        });
        services.AddSettingsSection<IGenericSettings, GenericSettings>(_configuration, nameof(GenericSettings));

        services.AddJwtAuth(_configuration);

        services.AddDomainServices();
        services.AddApplicationDependencies();
        services.AddSqlDataAccessLayerDependencies();
    }

    public void Configure(WebApplication app, IHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thermostat API v1");
            c.RoutePrefix = "swagger";
        });

        if (env.IsDevelopment())
        {
            // Request buffering & Serilog
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    if (httpContext.Items.TryGetValue("RequestBody", out var requestBody))
                        diagnosticContext.Set("RequestBody", requestBody);
                };
            });
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
            app.UseHttpsRedirection();

        }

        app.MapWhen(context => context.Request.Path == "/", appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.Redirect("/swagger");
                await context.Response.CompleteAsync();
            });
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}
