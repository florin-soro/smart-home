using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Thermostat.Api.Middlewares;

namespace Thermostat.Api.Extensions;
public static class DependencyRegistrationExtensions
{
    public static IServiceCollection AddSettingsSection<TInterface, TImplementation>(
    this IServiceCollection services,
    IConfiguration configuration,
    string sectionName)
    where TImplementation : class, TInterface, new()
    where TInterface : class
    {
        var section = configuration.GetSection(sectionName);
        var settingsInstance = new TImplementation();
        section.Bind(settingsInstance);

        services.AddSingleton<TInterface>(settingsInstance);

        return services;
    }

    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        //todo dedicate codig class
        var azureAdConfig = configuration.GetSection("AzureAd");
        var tenantId = azureAdConfig["TenantId"];
        var audience = azureAdConfig["Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.Authority = $"{azureAdConfig["Instance"]}{tenantId}";
         options.Audience = audience;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = $"https://sts.windows.net/{tenantId}/",
             ValidAudience = audience,
             RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
         };

         options.Events = new JwtBearerEvents
         {
             OnAuthenticationFailed = context =>
             {
                 Console.WriteLine($"JWT Auth Failed: {context.Exception.Message}");
                 return Task.CompletedTask;
             },
             OnTokenValidated = context =>
             {
                 var claims = context.Principal.Claims.Select(c => new { c.Type, c.Value });
                 Console.WriteLine("Claims in token:");
                 foreach (var c in claims) Console.WriteLine($" - {c.Type}: {c.Value}");
                 return Task.CompletedTask;
             }
             //for debug purposes
             //OnTokenValidated = context =>
             //{
             //    var claims = context.Principal?.Claims.Select(c => new { c.Type, c.Value }).ToList();
             //    Console.WriteLine("All claims in token:");
             //    foreach (var c in claims)
             //        Console.WriteLine($" - {c.Type}: {c.Value}");

             //    var roles = context.Principal?.FindAll("roles").Select(r => r.Value);
             //    Console.WriteLine($"Token roles: [{string.Join(", ", roles ?? new string[0])}]");
             //    return Task.CompletedTask;
             //},
         };
     });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));

            options.AddPolicy("SensorDataAccess", policy =>
                policy.RequireRole("sensor-push-data"));

            options.AddPolicy("AnyValidRole", policy =>
                policy.RequireRole("admin", "sensor-push-data"));
        });
        services.AddSingleton<IAuthorizationHandler, FunctionAppRoleHandler>();
    }

}
